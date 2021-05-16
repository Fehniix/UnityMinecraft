using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Provides efficient access to Resources (about 3 times faster than Resource.Load()!) by caching them upon load.
/// </summary>
public static class CachedResources
{
    private static Dictionary<string, Object> resourceCache = new Dictionary<string, Object>();

	/// <summary>
	/// Analogous to Resource.Load(). If the requested resource was not found in cache, it gets first loaded, stored and then returned.
	/// </summary>
    public static T Load<T>(string path) where T : Object
    {
        if (!resourceCache.ContainsKey(path))
            resourceCache[path] = Resources.Load<T>(path);
        return (T)resourceCache[path];
    }
}