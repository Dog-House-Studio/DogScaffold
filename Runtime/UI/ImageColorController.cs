using UnityEngine;
using UnityEngine.UI;

namespace DogScaffold
{
    /// <summary>
    /// ImageColorController is a controller component
    /// that will change the color of an Image ui component.
    /// </summary>
    public class ImageColorController : MonoBehaviour 
    {
        #region Public Variables
        public Color ImageColor => (Color)m_image?.color;
        #endregion

        #region Private Variables
        [SerializeField]
        private Image m_image = default(Image);
        #endregion
        
        #region Main Methods
        public void SetColor(Color color) => m_image.color = color;
        #endregion
    }
}
