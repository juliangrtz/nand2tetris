package vm

import (
	"bufio"
	"fmt"
	"io"
	"strconv"
	"strings"
)

type vmReader struct {
	reader    *bufio.Reader
	line      string
	operation string
	arg1      string
	arg2      int
	eof       bool
}

func newVmReader(stream *bufio.Reader) (vr *vmReader) {
	vr = new(vmReader)
	vr.reader = stream
	return vr
}

func (vr *vmReader) Next() (ok bool) {
	var err error
	for true {
		vr.line, err = vr.reader.ReadString('\n')
		if err == io.EOF {
			if vr.eof {
				return false
			}
			vr.eof = true
		} else if err != nil {
			return false
		}
		vr.line = strings.SplitN(vr.line, "//", 2)[0]
		vr.line = strings.TrimRight(vr.line, "\n\t ")

		if len(vr.line) == 0 {
			continue
		}

		split := strings.Split(vr.line, " ")

		vr.operation = split[0]
		switch {
		case len(split) > 2:
			vr.arg2, err = strconv.Atoi(split[2])
			if err != nil {
				panic(fmt.Sprintf("arg2 is not a number: %s", err.Error()))
			}
			fallthrough
		case len(split) > 1:
			vr.arg1 = split[1]
		}

		if vr.operation == "function" && vr.arg1 == "Sys.init" {
			continue
		}
		return true
	}
	return false
}
