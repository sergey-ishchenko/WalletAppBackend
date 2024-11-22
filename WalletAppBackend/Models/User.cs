﻿namespace WalletAppBackend.Models;

public sealed class User
{
	public int Id { get; set; }
	public string Email { get; set; }
	public string Name { get; set; }

	public Card Card { get; set; }
}