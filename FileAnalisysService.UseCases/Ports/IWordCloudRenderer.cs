namespace FileAnalisysService.UseCases.Ports;

public interface IWordCloudRenderer
{
    byte[] RenderPng(string text);
}