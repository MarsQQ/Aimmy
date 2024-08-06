﻿using System.Windows;
using System.Windows.Threading;
using Aimmy2.AILogic.Contracts;
using Aimmy2.Config;
using Aimmy2.Other;
using Class;
using Visuality;


namespace Aimmy2.AILogic.Actions;

public class OverlayAction : BaseAction
{
    private readonly DetectedPlayerWindow _playerOverlay = new();


    public override Task ExecuteAsync(Prediction[] predictions)
    {
        var closestPrediction = predictions.MinBy(p => p.Confidence);
        if (closestPrediction != null && Active)
        {
            UpdateOverlay(closestPrediction);
        }
        else
        {
            DisableOverlay();
        }
        return Task.CompletedTask;
    }


    private void UpdateOverlay(Prediction prediction)
    {
        var lastDetectionBox = prediction.TranslatedRectangle;
        Application.Current.Dispatcher.Invoke(() =>
        {
            var scalingFactorX = WinAPICaller.scalingFactorX;
            var scalingFactorY = WinAPICaller.scalingFactorY;
            var centerX = Convert.ToInt16(lastDetectionBox.X / scalingFactorX) + (lastDetectionBox.Width / 2.0);
            var centerY = Convert.ToInt16(lastDetectionBox.Y / scalingFactorY);

            if (AppConfig.Current.ToggleState.ShowAIConfidence)
            {
                _playerOverlay.DetectedPlayerConfidence.Opacity = 1;
                _playerOverlay.DetectedPlayerConfidence.Content = $"{Math.Round((prediction.Confidence * 100), 2)}%";

                var labelEstimatedHalfWidth = _playerOverlay.DetectedPlayerConfidence.ActualWidth / 2.0;
                _playerOverlay.DetectedPlayerConfidence.Margin = new Thickness(centerX - labelEstimatedHalfWidth, centerY - _playerOverlay.DetectedPlayerConfidence.ActualHeight - 2, 0, 0);
            }

            var showTracers = AppConfig.Current.ToggleState.ShowTracers;
            _playerOverlay.DetectedTracers.Opacity = showTracers ? 1 : 0;
            if (showTracers)
            {
                _playerOverlay.DetectedTracers.X2 = centerX;
                _playerOverlay.DetectedTracers.Y2 = centerY + lastDetectionBox.Height;
            }

            _playerOverlay.Opacity = AppConfig.Current.SliderSettings.Opacity;

            _playerOverlay.DetectedPlayerFocus.Opacity = 1;
            _playerOverlay.DetectedPlayerFocus.Margin = new Thickness(centerX - (lastDetectionBox.Width / 2.0), centerY, 0, 0);
            _playerOverlay.DetectedPlayerFocus.Width = lastDetectionBox.Width;
            _playerOverlay.DetectedPlayerFocus.Height = lastDetectionBox.Height;

            _playerOverlay.SetHeadRelativeArea(AppConfig.Current.ToggleState.ShowTriggerHeadArea ? prediction.HeadRelativeRect : null);
        });
    }

    private static void DrawDirect(Prediction prediction)
    {
        float scaleX = WinAPICaller.ScreenWidth / 640f;
        float scaleY = WinAPICaller.ScreenHeight / 640f;

        //PredictionDrawer.DrawPredictions(new []{prediction}, WinAPICaller.scalingFactorX, WinAPICaller.scalingFactorY);
        PredictionDrawer.DrawPredictions(new []{prediction}, scaleX, scaleY);
    }

    private void DisableOverlay()
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            _playerOverlay.Opacity = 0;
        });
    }
}