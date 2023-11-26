package cmd

import (
	"GoAssembler/pkg/asm"
	"GoAssembler/pkg/vm"
	"github.com/spf13/cobra"
	"os"
)

var buildCmd = &cobra.Command{
	Use:   "build",
	Short: "Translate and assemble",
	Args:  cobra.ExactArgs(1),
	RunE: func(cmd *cobra.Command, args []string) error {
		asmFile, err := os.CreateTemp("", "out.asm")
		if err != nil {
			return err
		}
		defer os.Remove(asmFile.Name())

		err = vm.TranslateVmDir(args[0], asmFile)
		if err != nil {
			return err
		}

		outFile, err := os.Create(outFileHackName)
		if err != nil {
			return err
		}
		defer outFile.Close()

		labels := asm.LabelHack(asmFile)

		err = asm.AssembleAsmFile(asmFile, outFile, labels)
		if err != nil {
			return err
		}
		return nil
	},
}

func init() {
	rootCmd.AddCommand(buildCmd)
}
