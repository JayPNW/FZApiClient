<#
.SYNOPSIS
    Reports today's Fangbreaker bonus as "none" for zones that never get a bonus (or maybe one was incorrectly reported at some point).

.DESCRIPTION
    Reports go through POST /api/client/bonuses and count as made by the API key's owner.

    Run it with -WhatIf first to see what it would send.

.PARAMETER ApiKey
    Your fangbreaker.zone API key. Defaults to the FZ_API_KEY environment variable.

.EXAMPLE
    .\Set-QuietZonesToNone.ps1 -WhatIf

.EXAMPLE
    $env:FZ_API_KEY = '...'; .\Set-QuietZonesToNone.ps1
#>
[CmdletBinding(SupportsShouldProcess)]
param(
    [string] $ApiKey = $env:FZ_API_KEY,
    [string] $BaseUrl = 'https://fangbreaker.zone'
)

$ErrorActionPreference = 'Stop'
[Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12

$ServerName = 'Fangbreaker'
$ServerSlug = 'fangbreaker'

$Zones = @(
    # "Siren's Grotto"
    # 'Shar Vahl'
    # 'Erudin Palace'
    # 'Tacvi, Seat of the Slaver'
    # 'Erudin'
    # 'Grobb'
    # 'Paineel'
    'The Bazaar'
    'The Nexus'

    #########  Shadows of Luclin
    'Shadow Haven'
    'The Deep'

    #########  Lost Dungeons of Norrath
    'Deepest Guk'
    'Deepest Guk: Ritualist of Hate'
    'Deepest Guk: The Curse Reborn'
    'Deepest Guk: The Rescue'
    "Miragul's Menagerie"
    "Miragul's Menagerie: Frozen Nightmare"
    "Miragul's Menagerie: Synarcana"
    'Mistmoore Catacombs'
    'Mistmoore Catacombs: Scion Lair of Fury'
    'Mistmoore Catacombs: The Asylum of Invoked Stone'
    'Rujarkin Hills'
    'Rujarkin Hills: Prison Break'
    'Rujarkin Hills: The Hidden Vale'
    'Takish-Hiz'
    'Takish-Hiz: Sunken Library'
    'Takish-Hiz: The Palace Grounds'
    'Takish-Hiz: Within the Compact'

    #########  Gates of Discord
    'Abysmal Sea'
    'Ikkinz: Chambers of Destruction'
    'Ikkinz: Trial of Glorification'
    'Ikkinz: Trial of Righteousness'
    'Ikkinz: Trial of Singular Might'
    'Ikkinz: Trial of Transcendence'
    'Ikkinz: Trial of Tri-Fates'
    'Ikkinz: Trial of Twin Struggles'
    'Sewers of Nihilia, Lair of Trapped Ones'
    'Sewers of Nihilia, Pool of Sludge'
    'Sewers of Nihilia, Purifying Plant'
    'Sewers of Nihilia, the Crematory'

    #########  Omens of War
    # 'Asylum of Anguish'
    'Catacombs of Dranik A'
    'Catacombs of Dranik B'
    'Catacombs of Dranik C'
    'Catacombs of Dranik: Redemption'
    # "Plane of Hate: Innoruuk's Realm"
    'Proving Grounds: The Mastery of Adaptation'
    'Proving Grounds: The Mastery of Corruption'
    'Proving Grounds: The Mastery of Destruction'
    'Proving Grounds: The Mastery of Efficiency'
    'Proving Grounds: The Mastery of Endurance'
    'Proving Grounds: The Mastery of Fear'
    'Proving Grounds: The Mastery of Foresight'
    'Proving Grounds: The Mastery of Hate'
    'Proving Grounds: The Mastery of Ingenuity'
    'Proving Grounds: The Mastery of Specialization'
    'Proving Grounds: The Mastery of Subversion'
    'Proving Grounds: The Mastery of Weaponry'
)

if (-not $ApiKey -and -not $WhatIfPreference) {
    throw 'No API key. Pass -ApiKey or set $env:FZ_API_KEY (or use -WhatIf for a dry run).'
}

$today = Invoke-RestMethod -Uri "$BaseUrl/api/servers/$ServerSlug/bonuses/today"
if (-not $today.ok) { throw "Reading today's bonuses failed: $($today.error)" }

$reported = 0; $skipped = 0; $failed = 0; $errors = 0
foreach ($zone in $Zones) {
    $entries = @($today.data | Where-Object { $_.name -eq $zone })
    if ($entries.Count -eq 0) {
        Write-Warning "$zone : not in today's zone list; skipping."
        $failed++
        continue
    }

    $active = @($entries | Where-Object { $_.state -eq 'ACTIVE' })
    $bonus = $active | Where-Object { $_.bonus -ne 'none' } | Select-Object -First 1
    if ($bonus) {
        Write-Warning ("$zone : ERROR - already has a bonus today ('$($bonus.bonusLabel)'). " +
            'This zone may not belong in the list; consider updating the script.')
        $errors++
        continue
    }
    if ($active.Count -gt 0) {
        Write-Host "$zone : already reported today as 'No Bonus'; skipping."
        $skipped++
        continue
    }

    if (-not $PSCmdlet.ShouldProcess($zone, "Report today's bonus as 'none' on $ServerName")) { continue }

    $body = @{ serverName = $ServerName; zoneName = $zone; bonusKey = 'none' } | ConvertTo-Json -Compress
    try {
        $result = Invoke-RestMethod -Method Post -Uri "$BaseUrl/api/client/bonuses" `
            -Headers @{ 'X-API-Key' = $ApiKey } `
            -ContentType 'application/json; charset=utf-8' `
            -Body ([Text.Encoding]::UTF8.GetBytes($body))
        Write-Host "$zone : reported 'none' (updated: $($result.data.updated))."
        $reported++
    }
    catch {
        $message = $_.ErrorDetails.Message
        if (-not $message) { $message = $_.Exception.Message }
        Write-Warning "$zone : report failed: $message"
        $failed++
    }
}

Write-Host ''
Write-Host "Reported: $reported   Already 'No Bonus' (skipped): $skipped   Has a bonus (error): $errors   Failed: $failed"
if ($errors) {
    Write-Error "$errors zone(s) already have a bonus today. The zone list in this script probably needs updating."
}
if ($errors -or $failed) { exit 1 }
