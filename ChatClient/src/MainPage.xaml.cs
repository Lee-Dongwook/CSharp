using SimpleChatClient.ViewModels;

namespace SimpleChatClient;

public partial class MainPage : ContentPage
{
    public MainPage() : this(
         Application.Current?.Handler?.MauiContext?.Services?.GetRequiredService<ChatViewModel>()
         ?? throw new InvalidOperationException("ChatViewModel not registered"))
    { }

    public MainPage(ChatViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;

        vm.Messages.CollectionChanged += (_, _) =>
        {
            if (MessagesView.ItemSource is not null)
            {
                var last = vm.Messages.LastOrDefault();
                if (last is not null)
                    MessagesView.ScrollTo(last, position: ScrollToPosition.End, animate: true);
            }
        };
    }

    private void OnCompleted(object? sender, EventArgs e)
    {
        if (BindingContext is ChatViewModel vm && vm.SendCommand.CanExecute(null))
        {
            vm.SendCommand.Execute(null);
        }
    }
    
    private void OnTextChanged(object? sender, TextChangedEventArgs e)
	{
		var text = e.NewTextValue ?? string.Empty;
		if (text.EndsWith("\n", StringComparison.Ordinal))
		{
			if (BindingContext is ChatViewModel vm)
			{
				vm.InputText = text.TrimEnd('\r', '\n');
				if (vm.SendCommand.CanExecute(null))
					vm.SendCommand.Execute(null);
			}
		}
	}

}