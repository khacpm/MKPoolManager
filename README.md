# MKPoolManager
[Unity Utility] 
Preload GameObject to reuse them later, avoiding to Instantiate them. 
Very useful for mobile platforms.

HOW TO USE:
Drag and drop your prefabs to GameObject that contain MKPoolManager.
Call "GetUnusedObject(Link_to_Prefabs)" to get Unused-Object (usually deactived).
Call "PreloadObject(Link_to_Prefabs)" to add Objects to pools.
Call "UnloadObjects(Link_to_Prefabs)" to release memory.
