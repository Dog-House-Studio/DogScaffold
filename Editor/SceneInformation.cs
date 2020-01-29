#if UNITY_EDITOR
namespace DogScaffold
{
    /// <summary>
    /// Struct that stores scene name and scene path pairs.
    /// </summary>
    public struct SceneInformation
    {
        private string projectRelativePath;
        private string sceneName;


        public string ProjectRelativePath
        {
            get { return projectRelativePath; }
        }

        public string SceneName
        {
            get { return sceneName; }
        }


        public SceneInformation(string projectRelativePath)
        {
            this.projectRelativePath = projectRelativePath;
            sceneName = System.IO.Path.GetFileNameWithoutExtension(projectRelativePath);
        }
    }
}

#endif
