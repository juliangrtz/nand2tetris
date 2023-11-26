//

// ungefähr O(n²)

// bubbleSort(Array A)
//   for (n=end_offset; n>1; --n){
//     for (i=0; i<n-1; ++i){
//       if (A[i] - A[i+1] > 0){
//         A.swap(i, i+1)
//       }
//     }
//   }

// Eingaben: R0 ist Array-Anfang (Offset)
//			 R1 ist Array-Länge

@R0
D=M // Array-Anfang

@R1
M=D+M // R1 += R0

(outer)
	@R1 
	MD=M-1 // --n

	@R0
	D=D-M
	@end
	D;JEQ 

	@R0
	D=M

	(inner)
		A=D // arr[0]
		D=M

		A=A+1 // arr[1]

		// swap begin
		// A = D, B = M
		D=D+M
		M=D-M
		D=D-M

		A=A-1
		M=D
		// swap end
		
		D=A+1

		@R2
		M=D

		@R1
		D=M-D

		@outer
		D;JEQ // i < n-1

		@R2
		D=M

		@inner
		0;JMP

(end)

@end
0;JMP


