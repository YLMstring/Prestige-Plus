using BlueprintCore.Utils;
using Kingmaker.Localization;
using Kingmaker.UI.SettingsUI;
using System.Text;
using UnityEngine;
using static Kingmaker.UI.KeyboardAccess;
using ModMenu;
using ModMenu.Settings;

namespace PrestigePlus.Menu
{
#if DEBUG

  public class TestSettings
  {
    private static readonly string RootKey = "mod-menu.test-settings";
    private enum TestEnum
    {
      First,
      Second,
      Third,
      Last
    }

    private class UISettingsEntityDropdownTestEnum : UISettingsEntityDropdownEnum<TestEnum> { }

    private static readonly LogWrapper Logger = LogWrapper.Get("PrestigePlus");
    internal void Initialize()
    {
      ModMenu.ModMenu.AddSettings(
        SettingsBuilder.New(RootKey, CreateString("title", "Prestige Plus"))
          .AddDefaultButton(OnDefaultsApplied)
          .AddButton(
            Button.New(
              CreateString("button-desc", "Restart the game to apply changes!"), CreateString("button-text", "Do Not Turn Any Chosen Features Off"), OnClick))
          .AddToggle(
            Toggle.New(GetKey("grave"), defaultValue: true, CreateString("toggle-desc1", "Agent of the Grave"))
              .ShowVisualConnection()
              .OnValueChanged(OnToggle))
          .AddToggle(
            Toggle.New(GetKey("chevalier"), defaultValue: true, CreateString("toggle-desc2", "Chevalier"))
              .ShowVisualConnection()
              .OnValueChanged(OnToggle))
          .AddToggle(
            Toggle.New(GetKey("crusader"), defaultValue: true, CreateString("toggle-desc3", "Inheritor’s Crusader"))
              .ShowVisualConnection()
              .OnValueChanged(OnToggle))
          .AddToggle(
            Toggle.New(GetKey("dancer"), defaultValue: true, CreateString("toggle-desc4", "Shadowdancer (Defensive Roll)"))
              .ShowVisualConnection()
              .OnValueChanged(OnToggle)));
    }
        public bool CheckToggle1()
        {
            Logger.Info("Checking toggle");
            return ModMenu.ModMenu.GetSettingValue<bool>(GetKey("grave"));
        }

        public bool CheckToggle2()
        {
            Logger.Info("Checking toggle");
            return ModMenu.ModMenu.GetSettingValue<bool>(GetKey("chevalier"));
        }

        public bool CheckToggle3()
        {
            Logger.Info("Checking toggle");
            return ModMenu.ModMenu.GetSettingValue<bool>(GetKey("crusader"));
        }

        public bool CheckToggle4()
        {
            Logger.Info("Checking toggle");
            return ModMenu.ModMenu.GetSettingValue<bool>(GetKey("dancer"));
        }

        private void OnKeyPress()
    {
      Logger.Info($"Key was pressed!");
    }

    private void OnClick()
    {
      var log = new StringBuilder();
      log.AppendLine("Current settings: ");
      ///log.AppendLine($"-Toggle: {CheckToggle()}");
      log.AppendLine($"-Dropdown: {ModMenu.ModMenu.GetSettingValue<TestEnum>(GetKey("dropdown"))}");
      log.AppendLine($"-Default Slider Float: {ModMenu.ModMenu.GetSettingValue<float>(GetKey("float-default"))}");
      log.AppendLine($"-Slider Float: {ModMenu.ModMenu.GetSettingValue<float>(GetKey("float"))}");
      log.AppendLine($"-Default Slider Int: {ModMenu.ModMenu.GetSettingValue<int>(GetKey("int-default"))}");
      log.AppendLine($"-Slider Int: {ModMenu.ModMenu.GetSettingValue<int>(GetKey("int"))}");
      Logger.Info(log.ToString());
    }

    private void OnDefaultsApplied()
    {
      Logger.Info("Defaults were applied!");
    }

    private void OnToggle(bool value)
    {
      Logger.Info($"Toggle switched to {value}");
    }

    private void OnSliderFloatChanged(float value)
    {
      Logger.Info($"Float slider changed to {value}");
    }

    private static LocalizedString CreateString(string partialKey, string text)
    {
      return Helpers.CreateString(GetKey(partialKey), text);
    }

    private static string GetKey(string partialKey)
    {
      return $"{RootKey}.{partialKey}";
    }
  }
#endif
}
