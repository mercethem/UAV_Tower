dump1090.exe --interactive --net --net-ro-port 30002 --net-beast --mlat --gain 48.0 --quiet --ppm 53

:: 10 saniye bekle
timeout /t 10 /nobreak >nul

:loop
curl http://127.0.0.1:8080/data.json | jq '.' | findstr "MSG,1, MSG,3, MSG,5," >> C:\Users\merce\OneDrive\Masaüstü\dump1090-win.1.10.3010.14 - Copy\test.txt
timeout /t 1 /nobreak >nul

:: Kullanıcıdan bir tuşa basmasını bekle
echo Programı durdurmak için herhangi bir tuşa basın...
pause >nul

exit
