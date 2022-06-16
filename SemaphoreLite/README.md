### Version 1.0.5.0
- [x] `IsTakenAsync` (Default)
- [x] `IsTakenAsyncNoDelay` (Uses More CPU for very little benefit)
- [x] `IsTakenAsyncUseOldDelay` (Uses Task.Delay(1)) for the Delay)
- [x] Recreate `Delay` from scratch using operating system ticks to remove padding
- [x] Task will now run to completion behind the lock before returning True
- [x] Lightweight Semiphore
