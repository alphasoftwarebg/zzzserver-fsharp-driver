# zzzserver-fsharp-driver
F# driver for **ZZZ Base** through ZZZ Server

***ZZZClient.fs*** - the file with driver function implementation  

**sample usage:**
```fsharp
ZZZProgram(
	"localhost",					// ZZZ Server host
	3333,						// ZZZ Server port
	"#[cout;Hello World from ZZZServer!]")		// ZZZ Base sample program
```

Returned result encoded in the utf-8
