MSBuild.SonarQube.Runner.exe begin /k:"rosolko" /n:"WebDriverManager.Net" /v:"2.0.0"
msbuild /t:Rebuild
MSBuild.SonarQube.Runner.exe end