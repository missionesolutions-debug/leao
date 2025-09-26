@echo off
echo 🚀 Preparando build para producao - www.leaoai.com.br
echo =====================================================

cd /d "%~dp0"
python build_production.py

echo.
echo ✅ Build concluida!
echo 📁 Arquivos prontos na pasta 'dist'
echo 🌐 Configurado para: www.leaoai.com.br
echo.
echo 📝 Proximo passo: Envie a pasta 'dist' para o servidor
pause