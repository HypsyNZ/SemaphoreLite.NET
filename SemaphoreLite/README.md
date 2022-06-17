### Version 1.0.5.1
- [x] Change default behavior of Methods so the default method uses minimal CPU usage
- [x] `IsTakenAsync` (Default)
- [x] `IsTakenAsyncUseFastDelay` (Uses Delay(1)) for the Delay, Faster, Uses More CPU)
- [x] `IsTakenAsyncNoDelay` (Slightly Faster than NewDelay, but uses A lot more CPU)

### Version 1.0.5.0
- [x] Recreate `Delay` from scratch using operating system ticks to remove padding (Uses more CPU)
- [x] Task will now run to completion behind the lock before returning True
- [x] Lightweight Semiphore
