# Pegginglin - Buttplug.io mod for Peglin

This is a simple BepInEx mod for Peglin with the following features:

- Runs vibrator for 100ms whenever a peg is hit

## How to use

- Install Mod manually or using r2modman
- Start [Intiface Central](https://intiface.com/central)
- Start Peglin
- Soon after install, Intiface Central should show "Pegginglin" as connected
- Mod will currently vibrate any vibration capable hardware currently connected to Intiface Central

## Source

[Pegginglin source is available on github](https://github.com/qdot/pegginglin).

Note that the project is currently set up against the developer's local configuration, as nuget
Peglin packages are currently out of date.

Due to incompatibilities in the Nuget Buttplug packages and the version of .Net Peglin uses, building against the Buttplug C# library will require a local clone of the [buttplug-csharp repo](https://github.com/buttplugio/buttplug-csharp), with the `no-tuple` branch checked out.
