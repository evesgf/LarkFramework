@echo off
rmdir %~dp0"Assets\LarkTools"
mklink /d %~dp0"Assets\CommonPack" %~dp0"..\CommonPack\Assets\CommonPack"
