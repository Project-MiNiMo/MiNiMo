using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

public class ProducePanel : UIBase
{
    [SerializeField] private ProducePrimaryBack _producePrimaryBack;
    [SerializeField] private ProduceAdvancedBack _produceAdvancedBack;
    
    private ProduceManager _produceManager;
    
    public override void Initialize()
    {
        _produceManager = App.GetManager<ProduceManager>();
        
        _produceManager.IsProducing
            .Subscribe((isProducing) =>
            {
                gameObject.SetActive(isProducing);
            }).AddTo(gameObject);
    }

    private void Update()
    {
        if (!gameObject.activeSelf)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            _produceManager.DeactiveProduce();
        }
    }

    private void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);

        if (isActive)
        {
            var id = _produceManager.CurrentProduceObject.Data.ID;

            if (string.Equals(id, "Building_Farm") || string.Equals(id, "Building_Orchard"))
            {
                _producePrimaryBack.SetActive(true);
                _produceAdvancedBack.SetActive(false);
            }
            else
            {
                _producePrimaryBack.SetActive(false);
                _produceAdvancedBack.SetActive(true);
            }
        }
    }
}