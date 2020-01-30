using System.Collections.Generic;
using System;
using UnityEngine;
using System.Diagnostics;

namespace DogScaffold
{
    /// <summary>
    /// The Service Locator is a singleton
    /// class that allows for the registering
    /// of callbacks and access to individual
    /// services.
    /// </summary>
	public sealed class ServiceLocator 
	{
#region Private Variables
		private static ServiceLocator m_instance;

        private static Dictionary < string, object > m_serviceDictionary
        {
            get
            {
                if (m_instance == null) Initialize();
                return serviceDictionary;
            }

            set{ serviceDictionary = value; }
        }
		
        private static Dictionary < string, List<Action>> m_callbackDictionary
        {
            get
            {
                if (m_instance == null) Initialize();
                return callbackDictionary;
            }

            set{ callbackDictionary = value; }
        }

        private static Dictionary<string, object>       serviceDictionary;
        private static Dictionary<string, List<Action>> callbackDictionary;
        //private ServiceReference<ILogService> 
#endregion

#region Main Methods
        public static void Register < T >(T service)
		{
            if (CheckServiceIsRegistered<T>())  ReplaceServiceInstance<T>(service);
            if (!CheckServiceIsRegistered<T>()) SetServiceInstance    <T>(service);
            DispatchRegistrationHandles <T>();
		}

        public static void Register(string typeName, object service)
        {
            if (CheckServiceIsRegisterd(typeName)) ReplaceServiceInstance(typeName, service);
            if (!CheckServiceIsRegisterd(typeName)) SetServiceInstance(typeName, service);
            DispatchRegistrationHandles(typeName);
        }

		public static void Unregister<T>(object Service)
		{
			if (!CheckServiceIsRegistered<T> ())    return;
            if (FetchService<T>() == null)          return;
            if (!FetchService<T>().Equals(Service)) return;

            UnregisterService<T>();
		}

        public static void Unregister(string typeName, object service)
        {
            if (!CheckServiceIsRegisterd(typeName)) return;
            if (FetchServiceObject(typeName) == null) return;
            if (!FetchServiceObject(typeName).Equals(service)) return;

            UnregisterService(typeName);
        }

		public static T FetchService <T>() =>
                CheckServiceIsRegistered<T>() 
                    ? FetchServiceInstance<T>() 
                    : default(T);

        private static object FetchServiceObject(string typeName)
        {
            return CheckServiceIsRegisterd(typeName)
                ? FetchServiceInstance(typeName)
                : null;
        }

		public static void AddRegistrationHandler<T>(Action callback)
		{
			if (CheckServiceIsRegistered<T> ()) callback ();
			AddHandle<T> (callback);
		}
#endregion

#region Utility Methods
        private static void ReplaceServiceInstance<T>(T NewService)
		{
            SendServiceReplacementWarning<T>();
            m_serviceDictionary[typeof(T).Name] = NewService;
		}

        private static void ReplaceServiceInstance(string typeName, object newService)
        {
            SendServiceReplacementWarning(typeName);
            m_serviceDictionary[typeName] = newService;
        }

		private static void DispatchRegistrationHandles<T>()
		{
            if (!CheckHandleIsRegistered<T>()) return;
		    DispatchHandles (m_callbackDictionary [typeof(T).Name]);
		}

        private static void DispatchRegistrationHandles(string typeName)
        {
            if (!CheckHandleIsRegistered(typeName)) return;
            DispatchHandles(m_callbackDictionary[typeName]);
        }
#endregion

#region Low Level Functions
		private static void RemoveHandles<T>()
		{
			if (!CheckHandleIsRegistered<T> ()) return;
			m_callbackDictionary.Remove (typeof(T).Name);
		}

		private static void Initialize()
		{
			if (m_instance != null) return;

			m_instance           = new ServiceLocator ();
			serviceDictionary    = new Dictionary < string, object >();
			callbackDictionary   = new Dictionary < string, List<Action>> ();
		}

		private static void AddHandle<T>(Action Handle)
		{
			if(!CheckHandleIsRegistered<T>())
				m_callbackDictionary [typeof(T).Name] = new List<Action> ();

			m_callbackDictionary [typeof(T).Name].Add (Handle);
		}

		private static void DispatchHandles(List<Action> Handles)
		{
			foreach (Action handle in Handles) handle?.Invoke();
		}

        private static bool CheckHandleIsRegistered<T>()        =>
            m_callbackDictionary.ContainsKey(typeof(T).Name);

        private static bool CheckHandleIsRegistered(string typeName) =>
            m_callbackDictionary.ContainsKey(typeName);

		private static void UnregisterService<T>()              =>
			m_serviceDictionary [typeof(T).Name] = default(T);

        private static void UnregisterService(string typeName) =>
            m_serviceDictionary[typeName] = null;

        [Conditional("ENABLE_LOGS")]
        private static void SendServiceReplacementWarning<T>() =>
            UnityEngine.Debug.LogWarning($"Service : {typeof(T).Name} is being replaced.");

        [Conditional("ENABLE_LOGS")]
        private static void SendServiceReplacementWarning(string typeName) =>
            UnityEngine.Debug.LogWarning($"Service : {typeName} is being replaced.");

        private static bool CheckServiceIsRegistered<T>()       =>
            m_serviceDictionary.ContainsKey(typeof(T).Name);

        private static bool CheckServiceIsRegisterd(string typeName) =>
            m_serviceDictionary.ContainsKey(typeName);

        private static T FetchServiceInstance<T>()              =>
            (T)m_serviceDictionary[typeof(T).Name];

        private static object FetchServiceInstance(string typeName) =>
            m_serviceDictionary[typeName];

        private static void SetServiceInstance<T>(T instance)   =>
            m_serviceDictionary[typeof(T).Name] = instance;

        private static void SetServiceInstance(string typeName, object service) =>
            m_serviceDictionary[typeName] = service;
#endregion
    }
}
