package cmd

import (
	"GoAssembler/pkg/asm"
	"github.com/spf13/cobra"
	"os"
)

var outFileHackName string

var assembleCmd = &cobra.Command{
	Use:   "assemble",
	Short: "Assemble a hack executable from asm",
	Args:  cobra.ExactArgs(1),
	RunE: func(cmd *cobra.Command, args []string) error {
		inFile, err := os.Open(args[0])
		if err != nil {
			return err
		}
		defer inFile.Close()

		outFile, err := os.Create(outFileHackName)
		if err != nil {
			return err
		}
		defer outFile.Close()

		labels := asm.LabelHack(inFile)

		err = asm.AssembleAsmFile(inFile, outFile, labels)
		if err != nil {
			return err
		}
		return nil
	},
}

func init() {
	rootCmd.AddCommand(assembleCmd)
	assembleCmd.PersistentFlags().StringVarP(&outFileHackName, "outfile", "o", "./out.hack", "Output file name")
}
