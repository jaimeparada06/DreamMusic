SET IDENTITY_INSERT [dbo].[Generos] ON
INSERT INTO [dbo].[Generos] ([GeneroID], [Nombre]) VALUES (3, N'Electrónica')
INSERT INTO [dbo].[Generos] ([GeneroID], [Nombre]) VALUES (2, N'Pop')
INSERT INTO [dbo].[Generos] ([GeneroID], [Nombre]) VALUES (1, N'Rap')
INSERT INTO [dbo].[Generos] ([GeneroID], [Nombre]) VALUES (4, N'Rock')
SET IDENTITY_INSERT [dbo].[Generos] OFF

SET IDENTITY_INSERT [dbo].[Discos] ON
INSERT INTO [dbo].[Discos] ([DiscoId], [PrecioDeDevolucion], [PrecioDeCompra], [GeneroID], [PrecioComprarProveedor], [FechaLanzamiento], [PrecioDeRestauracion], [Titulo], [Artista],[CantidadCompra],[CantidadDevolucion],[CantidadRestauracion]) VALUES (1, 30, 30, 1, 30, N'2020-10-20 00:00:00', 30, N'Scorpion', N'Drake',10,10,10)
INSERT INTO [dbo].[Discos] ([DiscoId], [PrecioDeDevolucion], [PrecioDeCompra], [GeneroID], [PrecioComprarProveedor], [FechaLanzamiento], [PrecioDeRestauracion], [Titulo], [Artista],[CantidadCompra],[CantidadDevolucion],[CantidadRestauracion]) VALUES (2, 23, 23, 1, 23, N'2014-09-13 00:00:00', 23, N'UnderPressure', N'Logic',10,10,10)
INSERT INTO [dbo].[Discos] ([DiscoId], [PrecioDeDevolucion], [PrecioDeCompra], [GeneroID], [PrecioComprarProveedor], [FechaLanzamiento], [PrecioDeRestauracion], [Titulo], [Artista],[CantidadCompra],[CantidadDevolucion],[CantidadRestauracion]) VALUES (3, 45, 45, 4, 45, N'2018-05-27 00:00:00', 45, N'Bohemian Rhapsody', N'Queen',10,10,10)
INSERT INTO [dbo].[Discos] ([DiscoId], [PrecioDeDevolucion], [PrecioDeCompra], [GeneroID], [PrecioComprarProveedor], [FechaLanzamiento], [PrecioDeRestauracion], [Titulo], [Artista],[CantidadCompra],[CantidadDevolucion],[CantidadRestauracion]) VALUES (4, 29, 29, 2, 29, N'1982-04-19 00:00:00', 29, N'Thriller', N'Michael Jackson',10,10,10)
SET IDENTITY_INSERT [dbo].[Discos] OFF

SET IDENTITY_INSERT [dbo].[Compras] ON
INSERT INTO [dbo].[Compras] ([CompraId], [PrecioTotal], [FechaCompra], [Direccion], [ClienteId], [MetodoDePagoID]) VALUES (1, 23, N'2020-08-18 00:00:00', NULL, NULL, NULL)
INSERT INTO [dbo].[Compras] ([CompraId], [PrecioTotal], [FechaCompra], [Direccion], [ClienteId], [MetodoDePagoID]) VALUES (2, 45, N'2012-12-21 00:00:00', NULL, NULL, NULL)
INSERT INTO [dbo].[Compras] ([CompraId], [PrecioTotal], [FechaCompra], [Direccion], [ClienteId], [MetodoDePagoID]) VALUES (3, 18, N'2001-01-01 00:00:00', NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[Compras] OFF


SET IDENTITY_INSERT [dbo].[ItemDeCompras] ON
INSERT INTO [dbo].[ItemDeCompras] ([Id], [DiscoId], [CompraId], [CantidadCompra]) VALUES (1, 1, 1, 1)
INSERT INTO [dbo].[ItemDeCompras] ([Id], [DiscoId], [CompraId], [CantidadCompra]) VALUES (2, 2, 2, 1)
INSERT INTO [dbo].[ItemDeCompras] ([Id], [DiscoId], [CompraId], [CantidadCompra]) VALUES (3, 4, 3, 1)
SET IDENTITY_INSERT [dbo].[ItemDeCompras] OFF