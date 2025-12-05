using System.Collections.Generic;
using System.Linq;
using Gameplay.Data;
using Interfaces.Services;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    [DefaultExecutionOrder(51)]
    public class BrushSelectView : View<BrushSelectView>
    {
        [SerializeField] private BrushSelectData m_BrushSelectData;
        [SerializeField] private BrushItem m_BrushItemPrefab;
        [SerializeField] private BrushItem m_SelectedSkinItem;
        [SerializeField] private Transform gridContainer;
        [SerializeField] private List<Image> m_Images = new List<Image>();
        [SerializeField] private Button m_BackButton;
        [SerializeField] private Shader m_StencilShader;
        private List<BrushItem> m_SkinItems = new List<BrushItem>();
        private IStatsService m_StatsService;

        [Inject]
        public void Construct(IStatsService statsService, IFeaturesService featuresService)
        {
            m_StatsService = statsService;
        }
        
        protected void Start()
        {
            CreateGrid();
            var favoriteBrush = m_BrushSelectData.Brushes.TryGetValue(m_StatsService.FavoriteBrush, out var favoriteBrushTemp) 
                ? favoriteBrushTemp : m_BrushSelectData.Brushes.Values.First();
            var favoriteColor = m_BrushSelectData.Colors.TryGetValue(m_StatsService.FavoriteColor, out var favoriteColorTemp) 
                ? favoriteColorTemp : m_BrushSelectData.Colors.Values.First();
            ChangeSkin(new BrushItemData(
                favoriteBrush.m_BrushMenuPrefab.GetComponent<BrushMenu>(),
                favoriteColor.m_Colors[0],
                favoriteBrush.BrushID, favoriteColor.ColorId
            ));
            m_BackButton.onClick.AddListener(()=> GameService.ChangePhase(GamePhase.MAIN_MENU));
            m_SkinItems.ForEach(x=>x.Hide());
            m_SelectedSkinItem.Hide();
        }

        public void Enable()
        {
            MainMenuView.Instance.AddBrushButton(()=> GameService.ChangePhase(GamePhase.BRUSH_SELECT));
        }

        public void Disable()
        {
            MainMenuView.Instance.RemoveBrushButton();
            if (GameService?.currentPhase == GamePhase.BRUSH_SELECT)
            {
                GameService.ChangePhase(GamePhase.MAIN_MENU);
            }
        }

        private void SetSelectedBrush(BrushItemData brushItem)
        {
             m_SelectedSkinItem.Initialize(brushItem, null);
             foreach (var item in m_SkinItems)
             {
                 item.SetBackgroundColor(brushItem.Color);
             }

             foreach (var image in m_Images)
             {
                 image.color = brushItem.Color;
             }
             MainMenuView.Instance.SetTitleColor(brushItem.Color);
        }

        private void CreateGrid()
        {
            foreach (var brushData in m_BrushSelectData.Brushes.Values)
            {
                foreach (var colorData in m_BrushSelectData.Colors.Values)
                {
                    var brushMenu = brushData.m_BrushMenuPrefab.GetComponent<BrushMenu>();
                    var brushVariant = Instantiate(m_BrushItemPrefab, gridContainer);
                    brushVariant.Initialize(new BrushItemData(
                            brushMenu,
                            colorData.m_Colors[0],
                            brushData.BrushID,
                            colorData.ColorId),
                        ChangeSkin);
                    Renderer[] renderers = brushVariant.GetComponentsInChildren<Renderer>(true);

                    foreach (Renderer renderer in renderers)
                    {
                        Material newMaterial = new Material(renderer.material)
                        {
                            shader = m_StencilShader
                        };
                        renderer.material = newMaterial;
                    }

                    m_SkinItems.Add(brushVariant);
                }
            }
        }

        private void ChangeSkin(BrushItemData selectedBrush)
        {
            m_StatsService.FavoriteBrush = selectedBrush.BrushId;
            m_StatsService.FavoriteColor = selectedBrush.ColorId;
            GameService.SetColor(selectedBrush.ColorId);
            SetSelectedBrush(selectedBrush);
        }

        protected override void OnGamePhaseChanged(GamePhase gamePhase)
        {
            base.OnGamePhaseChanged(gamePhase);
            if (gamePhase == GamePhase.BRUSH_SELECT)
            {
                Transition(true);
                m_SkinItems.ForEach(x=>x.Show());
                m_SelectedSkinItem.Show();
                return;
            }

            if (gamePhase == GamePhase.MAIN_MENU && m_Visible)
            {
                Transition(false);
                m_SkinItems.ForEach(x=>x.Hide());
                m_SelectedSkinItem.Hide();
            }
        }

        public void SetTitleColor(Color color)
        {
            m_BackButton.image.color = color;
            m_SelectedSkinItem.ChangeBrushColor(color);
        }
    }
}