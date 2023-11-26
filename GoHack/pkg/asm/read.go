package asm

import (
	"GoAssembler/pkg/util"
	"bufio"
	"io"
	"os"
	"strings"
)

type asmReader struct {
	reader     *bufio.Reader
	skipLabels bool
	line       string
	eof        bool
}

func newAsmReader(file *os.File, skipLabels bool) (ar *asmReader) {
	ar = new(asmReader)
	file.Seek(0, io.SeekStart)
	ar.reader = bufio.NewReader(file)
	ar.skipLabels = skipLabels
	return ar
}

func (ar *asmReader) Next() (ok bool) {
	var err error
	for true {
		ar.line, err = ar.reader.ReadString('\n')
		if err == io.EOF {
			if ar.eof {
				return false
			}
			ar.eof = true
		} else if err != nil {
			return false
		}
		ar.line = strings.TrimRight(ar.line, "\n")
		ar.line = strings.Map(util.FilterWhitespace, ar.line)

		ar.line = strings.SplitN(ar.line, "//", 2)[0]

		if len(ar.line) == 0 {
			continue
		}
		if strings.HasPrefix(ar.line, "(") && ar.skipLabels {
			continue
		}
		return true
	}
	return false
}
