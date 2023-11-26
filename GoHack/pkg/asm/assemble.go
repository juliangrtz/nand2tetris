package asm

import (
	"bufio"
	"fmt"
	"os"
	"strconv"
	"strings"
)

var comps map[string]string
var dests map[string]string
var jumps map[string]string
var symbols map[string]int

func AssembleAsmFile(inputASM *os.File, outputHack *os.File, labels map[string]int) (err error) {
	varIndex := 16
	variables := make(map[string]int)
	ar := newAsmReader(inputASM, true)
	writer := bufio.NewWriter(outputHack)
	defer writer.Flush()

	for ar.Next() {
		switch ar.line[0] {
		case '@':
			var value int16
			// Check if value is:
			str := ar.line[1:]
			if n, ok := symbols[str]; ok { // a symbol
				value = int16(n)
			} else if n, ok = labels[str]; ok { // a label
				value = int16(n)
			} else if n, ok = variables[str]; ok { // a variable
				value = int16(n)
			} else {
				n, err = strconv.Atoi(str)
				if err != nil { // a new variable
					value = int16(varIndex)
					variables[str] = varIndex
					varIndex++
				} else { // a number
					value = int16(n)
				}
			}

			bits := strconv.FormatInt(int64(value), 2)
			_, err := fmt.Fprintf(writer, "0%015s\n", bits)
			if err != nil {
				return err
			}
			//_, err = writer.WriteString(fmt.Sprintf("0%015s\n", bits))

		default:
			dest := ""
			jump := ""
			comp := ""

			switch strings.Map(filterAsmSyntax, ar.line) {
			case "=;":
				dest = strings.Split(ar.line, "=")[0]
				inst := strings.Split(ar.line, ";")
				jump = inst[1]
				comp = strings.Split(inst[0], "=")[0]
			case "=":
				dest = strings.Split(ar.line, "=")[0]
				comp = strings.Split(ar.line, "=")[1]
			case ";":
				jump = strings.Split(ar.line, ";")[1]
				comp = strings.Split(ar.line, ";")[0]
			case "":
				comp = ar.line
			}
			if _, ok := comps[comp]; !ok {
				return fmt.Errorf("comparison \"%s\" is not known", comp)
			}
			if _, ok := dests[dest]; !ok {
				return fmt.Errorf("destination \"%s\" is not known", dest)
			}
			if _, ok := jumps[jump]; !ok {
				return fmt.Errorf("jump condition \"%s\" is not known", jump)
			}
			_, err = writer.WriteString("111" + comps[comp] + dests[dest] + jumps[jump] + "\n")
		}
	}
	return nil
}

func filterAsmSyntax(r rune) rune {
	switch r {
	case '=':
		return r
	case ';':
		return r
	default:
		return -1
	}
}

func init() {
	comps = make(map[string]string)
	dests = make(map[string]string)
	jumps = make(map[string]string)
	symbols = make(map[string]int)

	comps["0"] = "0101010"
	comps["1"] = "0111111"
	comps["-1"] = "0111010"
	comps["D"] = "0001100"
	comps["A"] = "0110000"
	comps["M"] = "1110000"
	comps["!D"] = "0001101"
	comps["!A"] = "0110001"
	comps["!M"] = "1110001"
	comps["-D"] = "0001111"
	comps["-A"] = "0110011"
	comps["-M"] = "1110011"
	comps["D+1"] = "0011111"
	comps["A+1"] = "0110111"
	comps["M+1"] = "1110111"
	comps["D-1"] = "0001110"
	comps["A-1"] = "0110010"
	comps["M-1"] = "1110010"
	comps["D+A"] = "0000010"
	comps["D+M"] = "1000010"
	comps["D-A"] = "0010011"
	comps["D-M"] = "1010011"
	comps["A-D"] = "0000111"
	comps["M-D"] = "1000111"
	comps["D&A"] = "0000000"
	comps["D&M"] = "1000000"
	comps["D|A"] = "0010101"
	comps["D|M"] = "1010101"

	dests[""] = "000"
	dests["M"] = "001"
	dests["D"] = "010"
	dests["MD"] = "011"
	dests["A"] = "100"
	dests["AM"] = "101"
	dests["AD"] = "110"
	dests["AMD"] = "111"

	jumps[""] = "000"
	jumps["JGT"] = "001"
	jumps["JEQ"] = "010"
	jumps["JGE"] = "011"
	jumps["JLT"] = "100"
	jumps["JNE"] = "101"
	jumps["JLE"] = "110"
	jumps["JMP"] = "111"

	for i := 0; i < 16; i++ {
		symbols[fmt.Sprintf("R%d", i)] = i
	}
	symbols["SCREEN"] = 16384
	symbols["KBD"] = 24576
	symbols["SP"] = 0
	symbols["LCL"] = 1
	symbols["ARG"] = 2
	symbols["THIS"] = 3
	symbols["THAT"] = 4
}
