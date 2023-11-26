package test

import (
	"GoAssembler/pkg/asm"
	"bufio"
	"fmt"
	"os"
	"testing"
)

const (
	pongAsm    = "/home/markus/dev/git/private/nand2tetris/GoHack/resources/Pong.asm"
	pongBigAsm = "/home/markus/dev/git/private/nand2tetris/GoHack/resources/PongBig.asm"
	pongHack   = "/home/markus/dev/git/private/nand2tetris/GoHack/resources/Pong.asm"
	outHack    = "/home/markus/dev/git/private/nand2tetris/GoHack/out/out.asm"
)

func TestCompileFile(t *testing.T) {
	in, err := os.Open(pongAsm)
	if err != nil {
		t.Fatal(err)
	}
	out, err := os.Create(outHack)
	if err != nil {
		t.Fatal(err)
	}

	labels := asm.LabelHack(in)
	err = asm.AssembleAsmFile(in, out, labels)
	if err != nil {
		t.Fatal(err)
	}
	out.Close()
	out, _ = os.Open(outHack)

	comp, err := os.Open(pongHack)
	if err != nil {
		t.Fatal(err)
	}
	line := 1
	outReader := bufio.NewReader(out)
	compReader := bufio.NewReader(comp)
	for true {
		o, err := outReader.ReadString('\n')
		if err != nil {
			break
		}
		c, err := compReader.ReadString('\n')
		if err != nil {
			break
		}
		if c != o {
			t.Fatal(fmt.Sprintf("At line %d: %s does not match expected %s", line, o, c))
		}
		line++
	}
}

func TestBigPong(t *testing.T) {
	in, err := os.Open(pongBigAsm)
	if err != nil {
		t.Fatal(err)
	}
	out, err := os.Create(outHack)
	if err != nil {
		t.Fatal(err)
	}

	labels := asm.LabelHack(in)
	err = asm.AssembleAsmFile(in, out, labels)
	if err != nil {
		t.Fatal(err)
	}
}

func BenchmarkBigPong(b *testing.B) {
	for i := 0; i < b.N; i++ {
		in, err := os.Open(pongBigAsm)
		if err != nil {
			b.Fatal(err)
		}
		out, err := os.Create(outHack)
		if err != nil {
			b.Fatal(err)
		}

		labels := asm.LabelHack(in)
		err = asm.AssembleAsmFile(in, out, labels)
		if err != nil {
			b.Fatal(err)
		}
	}
}
