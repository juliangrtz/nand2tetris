// This file is part of www.nand2tetris.org
// and the book "The Elements of Computing Systems"
// by Nisan and Schocken, MIT Press.
// File name: projects/04/Fill.asm

// Runs an infinite loop that listens to the keyboard input.
// When a key is pressed (any key), the program blackens the screen,
// i.e. writes "black" in every pixel;
// the screen should remain fully black as long as the key is pressed. 
// When no key is pressed, the program clears the screen, i.e. writes
// "white" in every pixel;
// the screen should remain fully clear as long as no key is pressed.

// Put your code here.
(loop)
@24575 // i = max screen
D=A
@R0 // R0 = i
M=D
@KBD // check KBD
D=M
@white
D;JEQ
@black
D;JGT

(white)
@R0 // D = i
D=M
A=D // Goto i
M=0 // SCREEN[i] = white
D=D-1 // D = i - 1
@R0
M=D // i = D
@SCREEN
D=D-A // D = i - SCREEN
@white
D;JGE // i >= 0
@loop
0;JMP

(black)
@R0
D=M
A=D
M=-1
D=D-1
@R0
M=D
@SCREEN
D=D-A
@black
D;JGE
@loop
0;JMP


@loop
0;JMP