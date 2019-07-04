# SharpDir
SharpDir is a simple code set to search both local and remote file systems for files using the same SMB process as dir.exe, which uses TCP port 445. This code is compatible with Cobalt Strike.

A big thanks to VladPVS for both his time and code!

```
C:\>SharpDir.exe

[-] Usage:
         <PATH|.|C:\|\\Computer\Share\> <FILE|payload.exe|passwords*|*.kdbx>


C:\>SharpDir.exe \\10.10.10.10\admin$ powershell.exe

\\10.10.10.10\admin$\System32\WindowsPowerShell\v1.0\powershell.exe
\\10.10.10.10\admin$\WinSxS\x86_microsoft-windows-powershell-exe_31bf3856ad364e35_10.0.14393.206_none_46fba042e79e4c99\powershell.exe
\\10.10.10.10\admin$\WinSxS\x86_microsoft-windows-powershell-exe_31bf3856ad364e35_10.0.14393.0_none_3a6bceab6087d6b5\powershell.exe

```
