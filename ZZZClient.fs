(*
	ZZZClient.fs

	Copyright 2017 ZZZ Ltd. - Bulgaria

	Licensed under the Apache License, Version 2.0 (the "License");
	you may not use this file except in compliance with the License.
	You may obtain a copy of the License at

	http://www.apache.org/licenses/LICENSE-2.0

	Unless required by applicable law or agreed to in writing, software
	distributed under the License is distributed on an "AS IS" BASIS,
	WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
	See the License for the specific language governing permissions and
	limitations under the License.
*)

#light
open System
open System.IO
open System.Net.Sockets
open System.Text

let ZZZProgram(serverHost: string, serverPort: int, program: string): string =
    let mutable result = ""
    try
        let mutable host = serverHost
        if serverHost.Equals("localhost") then
            host <- "127.0.0.1"

        let tc = new TcpClient(host, serverPort)
        let ns = tc.GetStream()

        if ns.CanWrite && ns.CanRead then
            // Do a simple write.
            let sendBytes = Encoding.UTF8.GetBytes(program + "\u0000")
            ns.Write(sendBytes, 0, sendBytes.Length) |> ignore

            // Read the NetworkStream into a byte buffer.
            let bytes: byte[] = Array.create tc.ReceiveBufferSize (byte(0))
            let returndata: StringBuilder = new StringBuilder()
            let mutable receivedBytes = ns.Read(bytes, 0, tc.ReceiveBufferSize)
            returndata.Append(Encoding.UTF8.GetString(bytes, 0, receivedBytes)) |> ignore
            while receivedBytes > 0 do
                receivedBytes <- ns.Read(bytes, 0, tc.ReceiveBufferSize)
                returndata.Append(Encoding.UTF8.GetString(bytes, 0, receivedBytes)) |> ignore

            // Return the data received from the host.
            result <- returndata.ToString()
        else
            if not(ns.CanRead) then
                Console.WriteLine("cannot not read data from this stream") |> ignore
            elif not(ns.CanWrite) then
                    Console.WriteLine("cannot write data to this stream") |> ignore
        tc.Close() |> ignore
    with 
    | ex -> Console.WriteLine(ex)
    result

[<EntryPoint>]
let main args =
    Console.OutputEncoding <- Encoding.UTF8

    let startTime = DateTime.UtcNow;

    Console.WriteLine(ZZZProgram("localhost", 3333, "#[cout;Hello World from ZZZServer!]")) |> ignore

    let stopTime = DateTime.UtcNow
    let elapsedTime = stopTime.Millisecond - startTime.Millisecond
    Console.WriteLine(elapsedTime.ToString() + " milliseconds") |> ignore

    Console.ReadKey() |> ignore
    0