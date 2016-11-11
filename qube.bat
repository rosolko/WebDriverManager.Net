MSBuild.SonarQube.Runner.exe begin /k:"rosolko" /n:"WebDriverManager.Net" /v:"1.3.0"
msbuild /t:Rebuild
MSBuild.SonarQube.Runner.exe end