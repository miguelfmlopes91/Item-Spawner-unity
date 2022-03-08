using NUnit.Framework;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace EditorProgramming
{
    
    public class CardTool : EditorWindow
    {
        [MenuItem("Tools/Card Editor Window")]
        public static void ShowWindow()
        {
            var window = GetWindow<CardTool>();
            window.titleContent = new GUIContent("Card Editor");
            window.minSize = new Vector2(800, 600);
        }

        private void OnEnable()
        {
            VisualTreeAsset original = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Scripts/Editor/CardTool.uxml");
            TemplateContainer treeAsset = original.CloneTree();
            rootVisualElement.Add(treeAsset);

            StyleSheet sheet =
                AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/Editor/CardToolStyles.uss");
            rootVisualElement.styleSheets.Add(sheet);
        }

        private void CreateCardListView()
        {
            FindAllCards(out CardData[] cards);

            ListView cardList = rootVisualElement.Query<ListView>("card-list").First();
            cardList.makeItem = () => new Label();
            cardList.bindItem = (element, i) => (element as Label).text = cards[i].name;

            cardList.itemsSource = cards;
            cardList.itemHeight = 10;
            cardList.selectionType = SelectionType.Single;

            cardList.onSelectionChange += (enumerable) =>
            {
                foreach (var it in enumerable)
                {
                    Box cardInfoBox = rootVisualElement.Query<Box>("card-info").First();
                    cardInfoBox.Clear();
                    
                    CardData card = it as CardData;

                    SerializedObject serializedCard = new SerializedObject(card);
                    SerializedProperty cardProperty = serializedCard.GetIterator();
                    cardProperty.Next(true);

                    while (cardProperty.NextVisible(false))
                    {
                        PropertyField prop = new PropertyField(cardProperty);
                        
                        //we disable the m_script property as we dob't allow anyone to change the script refernce
                        prop.SetEnabled(cardProperty.name != "m_Script");
                        prop.Bind(serializedCard);
                        cardInfoBox.Add(prop);

                        if (cardProperty.name == "cardImage")
                        {
                            prop.RegisterCallback<ChangeEvent<Object>>(
                                (evt => LoadCardImage(card.cardImage.texture)));
                        }
                    }
                    
                    LoadCardImage(card.cardImage.texture);
                }
            };
            cardList.Refresh();
        }

        private void FindAllCards(out CardData[] cards)
        {
            var guids = AssetDatabase.FindAssets("t:CardData");

            cards = new CardData[guids.Length];

            for (int i = 0; i < cards.Length; i++)
            {
                var path = AssetDatabase.GUIDToAssetPath(guids[i]);
                cards[i] = AssetDatabase.LoadAssetAtPath<CardData>(path);
            }
        }

        private void LoadCardImage(Texture texture)
        {
            var cardPreviewImage = rootVisualElement.Query<Image>("preview").First();
            cardPreviewImage.image = texture,
        }
    }
}
