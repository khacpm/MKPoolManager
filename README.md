# MKPoolManager
[Unity Utility] <br />
Preload GameObject to reuse them later, avoiding to Instantiate them. <br />
Very useful for mobile platforms.<br />
<br />
HOW TO USE:<br />
Drag and drop your prefabs to GameObject that contain MKPoolManager.<br />
Call "GetUnusedObject(Link_to_Prefabs)" to get Unused-Object (usually deactived).<br />
Call "PreloadObject(Link_to_Prefabs)" to add Objects to pools.<br />
Call "UnloadObjects(Link_to_Prefabs)" to release memory.<br />
