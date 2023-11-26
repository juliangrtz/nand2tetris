package asm

import (
	"os"
	"strings"
)

func LabelHack(inputASM *os.File) (labels map[string]int) {
	labels = make(map[string]int)
	instructions := 0

	reader := newAsmReader(inputASM, false)

	for reader.Next() {
		str := reader.line

		if strings.HasPrefix(str, "(") {
			label := strings.Trim(str, "()")
			if _, ok := labels[label]; !ok {
				labels[label] = instructions
			}
		} else if strings.Contains(str, "=") || strings.Contains(str, ";") || strings.Contains(str, "@") {
			instructions++
		}
	}
	return labels
}
