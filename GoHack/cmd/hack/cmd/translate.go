package cmd

import (
	"GoAssembler/pkg/vm"
	"github.com/spf13/cobra"
	"os"
)

var outFileAsmName string

var translateCmd = &cobra.Command{
	Use:   "translate",
	Short: "Translate VM code to Hack Assembly",
	Args:  cobra.ExactArgs(1),
	RunE: func(cmd *cobra.Command, args []string) (err error) {
		outFile, err := os.Create(outFileAsmName)
		if err != nil {
			return
		}
		defer outFile.Close()

		err = vm.TranslateVmDir(args[0], outFile)
		return
	},
}

func init() {
	rootCmd.AddCommand(translateCmd)
	translateCmd.PersistentFlags().StringVarP(&outFileAsmName, "outfile", "o", "./out.asm", "Output file name")
}
