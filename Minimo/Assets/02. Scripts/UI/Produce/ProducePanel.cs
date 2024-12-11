using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

public class ProducePanel : UIBase
{
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
}