package vm

import (
	"GoAssembler/pkg/util"
	"bufio"
	"fmt"
	"io/fs"
	"os"
	"path/filepath"
	"strconv"
	"strings"
)

var memorySymbols map[string]string
var binaryOperators []string
var unaryOperators []string
var conditionCount = 0

func TranslateVmDir(dirPath string, outputAsm *os.File) (err error) {
	outputWriter := bufio.NewWriter(outputAsm)
	defer outputWriter.Flush()
	sysVm, err := os.Open(filepath.Join(dirPath, "Sys.vm"))
	if err != nil {
		return err
	}
	_, err = outputWriter.WriteString(startAsm)
	if err != nil {
		return err
	}

	err = TranslateVmStream(bufio.NewReader(sysVm), outputWriter, "Sys")
	if err != nil {
		return err
	}

	err = filepath.WalkDir(dirPath, func(path string, d fs.DirEntry, e error) (err error) {
		if e != nil || d.IsDir() || !strings.HasSuffix(path, ".vm") || strings.HasSuffix(path, "Sys.vm") {
			return e
		}
		vmFile, err := os.Open(path)
		if err != nil {
			return err
		}
		reader := bufio.NewReader(vmFile)
		err = TranslateVmStream(reader, outputWriter, strings.TrimSuffix(filepath.Base(path), ".vm"))
		return
	})
	return
}

func TranslateVmStream(input *bufio.Reader, output *bufio.Writer, baseName string) (err error) {
	vr := newVmReader(input)

	for vr.Next() {
		err = vr.translateLine(baseName, output)
		if err != nil {
			return err
		}
	}
	return
}

func (vr *vmReader) translateLine(baseName string, writer *bufio.Writer) (err error) {
	prepare := prepareArithmetic(vr.operation)
	ifLabel := baseName + ".if" + strconv.Itoa(conditionCount)
	gotoLabel := baseName + ".label." + vr.arg1

	switch vr.operation {
	case "push":
		fmt.Fprintf(writer, "\n// push %s %d%s%s",
			vr.arg1, vr.arg2,
			loadValueAsm(vr.arg1, vr.arg2, baseName),
			pushASM,
		)
	case "pop":
		fmt.Fprintf(writer, "\n// pop %s %d%s%s",
			vr.arg1, vr.arg2,
			popAsm,
			storeValueAsm(vr.arg1, vr.arg2, baseName),
		)
	case "add":
		fmt.Fprintf(writer, "\n// add %s%s", prepare, addAsm)
	case "sub":
		fmt.Fprintf(writer, "\n// sub %s%s", prepare, subAsm)
	case "and":
		fmt.Fprintf(writer, "\n// and %s%s", prepare, andAsm)
	case "or":
		fmt.Fprintf(writer, "\n// or %s%s", prepare, orAsm)
	case "not":
		fmt.Fprintf(writer, "\n// not %s%s", prepare, notAsm)
	case "neg":
		fmt.Fprintf(writer, "\n// not %s%s", prepare, negAsm)
	case "eq":
		conditionCount++
		fmt.Fprintf(writer, "\n// eq %s", prepare)
		fmt.Fprintf(writer, comparisonAsm, ifLabel, eqAsm, ifLabel, ifLabel, ifLabel)
	case "lt":
		conditionCount++
		fmt.Fprintf(writer, "\n// lt %s", prepare)
		fmt.Fprintf(writer, comparisonAsm, ifLabel, ltAsm, ifLabel, ifLabel, ifLabel)
	case "gt":
		conditionCount++
		fmt.Fprintf(writer, "\n// gt %s", prepare)
		fmt.Fprintf(writer, comparisonAsm, ifLabel, gtAsm, ifLabel, ifLabel, ifLabel)
	case "label":
		fmt.Fprintf(writer, labelAsm, vr.arg1, gotoLabel)
	case "goto":
		fmt.Fprintf(writer, gotoAsm, vr.arg1, gotoLabel)
	case "if-goto":
		fmt.Fprintf(writer, ifGotoAsm, vr.arg1, gotoLabel)
	case "function":
		fmt.Fprintf(writer, "\n(func.%s)", vr.arg1)
		for i := 0; i < vr.arg2; i++ {
			fmt.Fprintf(writer, initLocalAsm, i)
		}
	default:
		err = fmt.Errorf("unknown operation: %s", vr.operation)
	}
	if err != nil {
		return
	}
	return nil
}

// load value from specified memory segment into D register
func loadValueAsm(segment string, index int, fileName string) (asm string) {
	switch segment {
	case "constant":
		asm = fmt.Sprintf(loadConstantAsm, index)
	case "temp":
		asm = fmt.Sprintf(loadAtAsm, strconv.Itoa(index+5))
	case "pointer":
		asm = fmt.Sprintf(loadAtAsm, strconv.Itoa(index+3))
	case "static":
		asm = fmt.Sprintf(loadAtAsm, strings.TrimSuffix(fileName, "vm")+strconv.Itoa(index))
	default:
		if symbol, ok := memorySymbols[segment]; ok {
			asm = fmt.Sprintf(loadSegmentAsm, symbol, index)
		}
	}
	return
}

// store D register in specified memory segment
func storeValueAsm(segment string, index int, fileName string) (asm string) {
	switch segment {
	case "temp":
		asm = fmt.Sprintf(storeAtAsm, strconv.Itoa(index+5))
	case "pointer":
		asm = fmt.Sprintf(storeAtAsm, strconv.Itoa(index+3))
	case "static":
		asm = fmt.Sprintf(storeAtAsm, strings.TrimSuffix(fileName, "vm")+strconv.Itoa(index))
	default:
		if symbol, ok := memorySymbols[segment]; ok {
			asm = fmt.Sprintf(storeSegmentAsm, symbol, index)
		}
	}
	return
}

// prepare stack pointer, D and A register for each operation
func prepareArithmetic(operation string) (asm string) {
	if util.StringInSlice(operation, binaryOperators) {
		asm = prepareBinaryAsm
	} else if util.StringInSlice(operation, unaryOperators) {
		asm = prepareUnaryAsm
	}
	return
}

func init() {
	memorySymbols = make(map[string]string, 4)
	memorySymbols["local"] = "LCL"
	memorySymbols["argument"] = "ARG"
	memorySymbols["this"] = "THIS"
	memorySymbols["that"] = "THAT"

	binaryOperators = []string{"add", "sub", "gt", "lt", "and", "or", "eq"}
	unaryOperators = []string{"not", "neg"}
}
