<#
.SYNOPSIS

Install dependencies for Appium UITests

.DESCRIPTION

This will install or update npm, appium and the following drivers: appium-windows-driver, uiautomator2, xcuitest and mac2

.PARAMETER appiumVersion

The Appium version to install

.PARAMETER windowsDriverVersion

The windows driver version to update or install

.PARAMETER androidDriverVersion

The uiautomator2 driver version to update or install

.PARAMETER iOSDriverVersion

The xcuitest driver version to update or install

.PARAMETER macDriverVersion

The mac2 driver version to update or install


.EXAMPLE

PS> .\appium-install.ps1 '2.1.1' 2.7.2 2.25.1 4.30.2 1.6.1

This would install or update Appium version 2.1.1, the windows driver 2.7.2, the uiautomator2 driver with 2.25.1, the xcuitest driver with 4.30.2 and mac2 driver with 1.6.1

Versions for these steps are pinned and ideally stay in sync with the script that initializes the XAMBOT agents.
Find the script for that on the DevDiv Azure DevOps instance, Engineering team, BotDeploy.PackageGeneration repo.
#>

param
(
    [string] $appiumVersion = '2.5.4',
    [string] $windowsDriverVersion = '2.12.23',
    [string] $androidDriverVersion = '3.5.1',
    [string] $iOSDriverVersion = '7.16.1',
    [string] $macDriverVersion = '1.17.3',
    [string] $logsDir = '../appium-logs'
)

Write-Output  "Welcome to the Appium installer"

Write-Output  "Node version"
node -v

$npmLogLevel = 'verbose'

# globally set npm loglevel
npm config set loglevel $npmLogLevel

# Create logs directory for npm logs if it doesn't exist
if (!(Test-Path $logsDir -PathType Container)) {
    New-Item -ItemType Directory -Path $logsDir
}

# If there's a ~/.appium folder, remove it as it can cause issues from v1
# it might also generally have caching issues for us with our runs
$appiumUserData = "$env:USERPROFILE/.appium"
if (Test-Path $appiumUserData) {
    Write-Output  "Removing $appiumUserData"
    Remove-Item -Path $appiumUserData -Force -Recurse
}

# Check for an existing appium install version
$appiumCurrentVersion = ""
try { $appiumCurrentVersion = appium -v | Out-String } catch { }

if ($appiumCurrentVersion) {
    Write-Output  "Existing Appium version $appiumCurrentVersion"
} else {
    Write-Output  "No Appium version installed"
}

# If current version does not match the one we want, uninstall and install the new version
if ($appiumCurrentVersion -ne $appiumVersion) {
    Write-Output  "Uninstalling appium $appiumCurrentVersion"
    npm uninstall --logs-dir=$logsDir --loglevel $npmLogLevel -g appium
    Write-Output  "Uninstalled appium $appiumCurrentVersion"

    Write-Output  "Installing appium $appiumVersion"
    npm install --logs-dir=$logsDir --loglevel $npmLogLevel -g appium@$appiumVersion
    write-Output  "Installed appium $appiumVersion"   
}

$existingDrivers = appium driver list --installed --json  | ConvertFrom-Json
Write-Output "List of installed drivers $existingDrivers"
if ($existingDrivers.windows) {
    Write-Output  "Uninstalling appium driver windows"
    appium driver uninstall windows
    Write-Output  "Uninstalled appium driver windows"
}

if ($existingDrivers.uiautomator2) {
    Write-Output  "Uninstalling appium driver uiautomator2"
    appium driver uninstall uiautomator2
    Write-Output  "Uninstalled appium driver uiautomator2"
}

if ($existingDrivers.xcuitest) {
    Write-Output  "Uninstalling appium driver xcuitest"
    appium driver uninstall xcuitest
    Write-Output  "Uninstalled appium driver xcuitest"
}

if ($existingDrivers.mac2) {
    Write-Output  "Uninstalling appium driver mac2"
    appium driver uninstall mac2
    Write-Output  "Uninstalled appium driver mac2"
}

$drivers = appium driver list --installed --json  | ConvertFrom-Json
Write-Output "List of installed drivers after cleaup $drivers"

Write-Output  "We will now install the appium drivers windows $windowsDriverVersion, uiautomator2 $androidDriverVersion, xcuitest $iOSDriverVersion and mac2 $macDriverVersion"

Write-Output  "Installing appium driver windows $windowsDriverVersion"
appium driver install --source=npm appium-windows-driver@$windowsDriverVersion
Write-Output  "Installed appium driver windows"

Write-Output  "Installing appium driver uiautomator2 $androidDriverVersion"
appium driver install uiautomator2@$androidDriverVersion
Write-Output  "Installed appium driver uiautomator2"

Write-Output  "Installing appium driver xcuitest $iOSDriverVersion"
appium driver install xcuitest@$iOSDriverVersion
Write-Output  "Installed appium driver xcuitest"

Write-Output  "Installing appium driver mac2 $macDriverVersion"
appium driver install mac2@$macDriverVersion
Write-Output  "Installed appium driver mac2"

Write-Output  "Check everything is installed correctly with appium doctor"
appium driver doctor appium-windows-driver || & { "ignore failure"; $global:LASTEXITCODE = 0 }
appium driver doctor uiautomator2 || & { "ignore failure"; $global:LASTEXITCODE = 0 }
appium driver doctor xcuitest || & { "ignore failure"; $global:LASTEXITCODE = 0 }
appium driver doctor mac2 || & { "ignore failure"; $global:LASTEXITCODE = 0 }

Write-Output  "Done, thanks!"
