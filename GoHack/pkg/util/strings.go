package util

import "unicode"

func FilterWhitespace(r rune) rune {
	if unicode.IsSpace(r) {
		return -1
	}
	return r
}

func StringInSlice(s string, l []string) bool {
	for _, s2 := range l {
		if s == s2 {
			return true
		}
	}
	return false
}
