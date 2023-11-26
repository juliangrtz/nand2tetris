package vm

//region load into D
const loadConstantAsm = `
@%d
D=A`

const loadSegmentAsm = `
@%s
D=M
@%d
A=D+A
D=M`

const loadAtAsm = `
@%s
D=M`

const pushASM = `
@SP
M=M+1
A=M-1
M=D`

//endregion

//region store D into
const storeAtAsm = `
@%s
M=D`

const storeSegmentAsm = `
@R13
M=D
@%s
D=M
@%d
D=D+A
@R14
M=D
@R13
D=M
A=A+1
A=M
M=D`

const popAsm = `
@SP
AM=M-1
D=M`

//endregion

//region operators
const addAsm = `
M=D+M`

const subAsm = `
M=M-D`

const andAsm = `
M=D&M`

const orAsm = `
M=D|M`

const eqAsm = `
D;JEQ`

const gtAsm = `
M-D;JLT`

const ltAsm = `
M-D;JGT`

const notAsm = `
M=!M`

const negAsm = `
M=-M`

//endregion

//region prepare stack operations
const prepareBinaryAsm = `
@SP
AM=M-1
D=M
A=A-1`

const prepareUnaryAsm = `
@SP
A=M-1`

const comparisonAsm = `
D=M-D
@%s.true
%s
@SP
A=M-1
M=0
@%s.false
0;JMP
(%s.true)
@SP
A=M-1
M=-1
(%s.false)`

//endregion

//region branching
const labelAsm = `
// label %s
(%s)`

const gotoAsm = `
// goto %s
@%s
0;JMP`

const ifGotoAsm = `
// if-goto %s
@SP
AM=M-1
D=M
@%s
D;JGT`

//endregion

const startAsm = `
@256
D=A
@SP
M=D`

const initLocalAsm = `
@%d
D=A
@LCL
A=D+M
M=0`
