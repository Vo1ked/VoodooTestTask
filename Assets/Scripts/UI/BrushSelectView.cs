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
        [SerializeField] public Transform gridContainer;
        [SerializeField] private Button m_BackButton;
        [SerializeField] private Shader m_StencilShader;
        private List<BrushItem> m_SkinItems = new List<BrushItem>();
        
        private IStatsService m_StatsService;
        private IFeaturesService m_FeaturesService;

        [Inject]
        public void Construct(IStatsService statsService, IFeaturesService featuresService)
        {
            m_StatsService = statsService;
            m_FeaturesService = featuresService;
        }
        
        protected override void Awake()
        {
            base.Awake();
            CreateGrid();
            var favoriteBrushId = Mathf.Min(m_StatsService.FavoriteBrush, m_BrushSelectData.Brushes.Count);
            var favoriteColorId = Mathf.Min(m_StatsService.FavoriteColor, m_BrushSelectData.Colors.Count);
            ChangeSkin(new BrushItemData(
                m_BrushSelectData.Brushes.First(x=> x.BrushID == favoriteBrushId).m_BrushMenuPrefab.GetComponent<BrushMenu>(),
                m_BrushSelectData.Colors.First(x => x.ColorId == favoriteColorId).m_Colors[0],
                favoriteBrushId, favoriteColorId
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
            if (GameService.currentPhase == GamePhase.BRUSH_SELECT)
            {
                GameService.ChangePhase(GamePhase.MAIN_MENU);
            }
        }

        private void SetSelectedBrush(BrushItemData brushItem)
        {
             m_SelectedSkinItem.Initialize(brushItem, null);
        }

        private void CreateGrid()
        {
            foreach (var brushData in m_BrushSelectData.Brushes)
            {
                foreach (var colorData in m_BrushSelectData.Colors)
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
            m_StatsService.FavoriteSkin = selectedBrush.BrushId;
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