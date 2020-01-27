@echo off
Setlocal
rem ***************************************************************************
rem * Copy script to tools
rem ***************************************************************************
powershell write-host -fore Cyan  ***************************************************************************
powershell write-host -fore Cyan "Copy script to tools"
powershell write-host -fore Cyan  ***************************************************************************
powershell write-host
copy Scripts Tools\SeedRepository\Scripts
pause