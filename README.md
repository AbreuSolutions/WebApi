# ASP.NET Core WebApi

## Tecnologias
- ASP.NET Core 3.1 WebApi
- .NET Core 3.1 / Standard 2.1 Libraries
- Newtonsoft / Json.NET
- REST Standards
- Bootstrap v5.0

## Descrição

Foi aplicado um Layout responsivo em (Bootstrap 5.0) e diversas Views onde o Controllers se comunica com uma API híbrida feita em método POST feito diversos tratamentos e comunicação com futuros "Clientes" que queiram fazer requisições.

Foi aplicado um formulário de acesso, em back-end onde foi tratado com "CookieAuthenticationDefaults / ClaimsIdentity" com "Claim" totalmente fora de padrões conhecidos e com seus dados criptografados.

![](https://github.com/AbreuSolutions/WebApi/blob/main/Dos/Login.jpg "Login")
![](https://github.com/AbreuSolutions/WebApi/blob/main/Dos/Claims.jpg "Claims")

Foi aplicado um Controllers "ApiDocumentacao" onde o usuário devidamente autenticado tem acesso a documentação onde explica de como utilizar o API, através de requisições.

![](https://github.com/AbreuSolutions/WebApi/blob/main/Dos/DocUsuarioCadastrar.jpg "API Documentação")

Foi aplicado dois Layouts responsivo em (Bootstrap 5.0) onde o 1º foi aplicado no portal e o 2º foi aplicado na documetação.

![](https://github.com/AbreuSolutions/WebApi/blob/main/Dos/Layouts.jpg "Layouts")

Foi aplicado uma forma de trabalhar com vários Bancos de Dados ao mesmo tempo, com identificação de ambiente (LocalHost e Produção).

![](https://github.com/AbreuSolutions/WebApi/blob/main/Dos/VariosBancosDeDados.jpg "Banco de Dados")

---

## Entidades
- [x] Produto / TB_Produtos
- [x] Usuario / TB_Usuario
- [x] UsuarioLogin / TB_Usuario_Login

## Ferramentas
- [x] Procedures
- [x] Util

## WebApi - Controllers
- [x] Home
- [x] ApiDocumentacao
- [x] ApiProduto
- [x] ApiUsuario


## Script gerar tabelas SQL Server

CREATE TABLE [dbo].[TB_Produtos] (
    [Id]                   INT             IDENTITY (1, 1) NOT NULL,
    [DataCadastro]         DATETIME2 (7)   NOT NULL,
    [CriadoPor]            VARCHAR (20)    NULL,
    [UltimaModificacaoPor] VARCHAR (20)    NULL,
    [UltimaModificacao]    DATETIME2 (7)   NULL,
    [Nome]                 VARCHAR (250)   NULL,
    [CodigoDeBarras]       VARCHAR (100)   NULL,
    [Descricao]            VARCHAR (MAX)   NULL,
    [Preco]                DECIMAL (18, 6) NOT NULL,
    [Status]               VARCHAR (1)     NULL,
    CONSTRAINT [PK_Produtos] PRIMARY KEY CLUSTERED ([Id] ASC)
);

---
CREATE TABLE [dbo].[TB_Usuario] (
    [Id]             INT           IDENTITY (1, 1) NOT NULL,
    [DataCadastro]   DATETIME      NOT NULL,
    [DataNascimento] DATETIME2 (7) NOT NULL,
    [Nome]           VARCHAR (50)  NOT NULL,
    [SobreNome]      VARCHAR (20)  NOT NULL,
    [Email]          VARCHAR (50)  NOT NULL,
    [Telefone]       VARCHAR (20)  NOT NULL,
    [Perfil]         VARCHAR (20)  NOT NULL,
    [Status]         VARCHAR (1)   NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

---
CREATE TABLE [dbo].[TB_Usuario_Login] (
    [Id]           INT           IDENTITY (1, 1) NOT NULL,
    [IdUsuario]    INT           NOT NULL,
    [DataCadastro] DATETIME      NOT NULL,
    [Email]        VARCHAR (50)  NOT NULL,
    [PasswordHash] VARCHAR (MAX) NOT NULL,
    [Status]       VARCHAR (1)   NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


## Script gerar procedures SQL Server

CREATE PROCEDURE [dbo].[SP_ProdutoAtualizar]
    @Id					  VARCHAR(11) = '0',
    @UltimaModificacaoPor VARCHAR(20) = '',
    @UltimaModificacao    VARCHAR(10) = '',
    @Nome                 VARCHAR(250) = '',
    @Descricao            VARCHAR(MAX) = '',
    @Preco                VARCHAR(24) = '',
    @Status               VARCHAR(1) = 'F'
AS

BEGIN

	DECLARE @ProcId VARCHAR(11) = '0';
	DECLARE @ProcStatus VARCHAR(3) = '';
	DECLARE @ProcDescricao VARCHAR(100) = '';

	BEGIN TRY

		UPDATE [dbo].[TB_Produtos] SET
			UltimaModificacaoPor = @UltimaModificacaoPor,
			UltimaModificacao = @UltimaModificacao,
			Nome = @Nome,
			Descricao = @Descricao,
			Preco = @Preco,
			Status = @Status
		WHERE Id = @Id;

		SET @ProcId = @Id;
		SET @ProcStatus = '000';
		SET @ProcDescricao = 'Requisição efetuada com sucesso.';
		SELECT @ProcId AS Id, @ProcStatus AS Status, @ProcDescricao AS Descricao;

	END TRY
	BEGIN CATCH

		IF(@@TRANCOUNT > 0)
		BEGIN
			SET @ProcId = '0';
			SET @ProcStatus = CAST(@@TRANCOUNT AS VARCHAR(3));
			SET @ProcDescricao = ERROR_MESSAGE();
			SELECT @ProcId AS Id, @ProcStatus AS Status, @ProcDescricao AS Descricao;
		END;

	END CATCH

END;

---
CREATE PROCEDURE [dbo].[SP_ProdutoCadastrar]
    @DataCadastro         VARCHAR(24) = '',
    @CriadoPor            VARCHAR(20) = '',
    @Nome                 VARCHAR(250) = '',
    @CodigoDeBarras       VARCHAR(100) = '',
    @Descricao            VARCHAR(MAX) = '',
    @Preco                VARCHAR(24) = '',
    @Status               VARCHAR(1) = 'F'
AS

BEGIN

	DECLARE @SetId INT = 0;
	DECLARE @ProcId INT = 0;
	DECLARE @ProcStatus VARCHAR(3) = '';
	DECLARE @ProcDescricao VARCHAR(100) = '';

	BEGIN TRY

		SELECT @SetId = ISNULL(Id, 0) FROM [dbo].[TB_Produtos] WHERE CodigoDeBarras = @CodigoDeBarras;

		IF(@SetId = 0)
			BEGIN
				INSERT INTO [dbo].[TB_Produtos] (
					DataCadastro, CriadoPor, Nome, CodigoDeBarras, Descricao, Preco, Status
				) VALUES (
					@DataCadastro,
					@CriadoPor,
					@Nome,
					@CodigoDeBarras,
					@Descricao,
					@Preco,
					@Status
				);

				SET @ProcId = @@IDENTITY;
				SET @ProcStatus = '000';
				SET @ProcDescricao = 'Requisição efetuada com sucesso.';
				SELECT @ProcId AS Id, @ProcStatus AS Status, @ProcDescricao AS Descricao;
			END;
		ELSE
			BEGIN
				SET @ProcId = 0;
				SET @ProcStatus = '901';
				SET @ProcDescricao = 'O código de barras já foi utilizado em outro produto.';
				SELECT @ProcId AS Id, @ProcStatus AS Status, @ProcDescricao AS Descricao;
			END;

	END TRY
	BEGIN CATCH

		IF(@@TRANCOUNT > 0)
		BEGIN
			SET @ProcId = 0;
			SET @ProcStatus = CAST(@@TRANCOUNT AS VARCHAR(3));
			SET @ProcDescricao = ERROR_MESSAGE();
			SELECT @ProcId AS Id, @ProcStatus AS Status, @ProcDescricao AS Descricao;
		END;

	END CATCH

END;

---
CREATE PROCEDURE [dbo].[SP_ProdutoListar]
    @Id					  VARCHAR(11) = '0',
    @UltimaModificacaoPor VARCHAR(20) = '',
    @Nome                 VARCHAR(250) = '',
    @Descricao            VARCHAR(MAX) = '',
    @Preco                VARCHAR(24) = '0.00',
    @Status               VARCHAR(1) = 'A'
AS

BEGIN

	DECLARE @SQL VARCHAR(MAX);
	DECLARE @ProcId VARCHAR(11) = '0';
	DECLARE @ProcStatus VARCHAR(3) = '';
	DECLARE @ProcDescricao VARCHAR(100) = '';

	BEGIN TRY

		SET @SQL = '';
		SET @SQL += 'SELECT * FROM [dbo].[TB_Produtos] WHERE Id > 0 ';
		IF(@Id <> '0') BEGIN SET @SQL += 'AND Id = ' + @Id + ' '; END;
		ELSE
			IF(@UltimaModificacaoPor <> '') BEGIN SET @SQL += 'AND UltimaModificacaoPor LIKE ''%' + @UltimaModificacaoPor + '%'' '; END;
			IF(@Nome <> '') BEGIN SET @SQL += 'AND Nome LIKE ''%' + @Nome + '%'' '; END;
			IF(@Descricao <> '') BEGIN SET @SQL += 'AND Descricao LIKE ''%' + @Descricao + '%'' '; END;
			IF(@Status <> 'T') BEGIN SET @SQL += 'AND Status = ''' + @Status + ''' '; END;

		EXECUTE(@SQL);
		--PRINT(@SQL);

	END TRY
	BEGIN CATCH

		IF(@@TRANCOUNT > 0)
		BEGIN
			SET @ProcId = '0';
			SET @ProcStatus = CAST(@@TRANCOUNT AS VARCHAR(3));
			SET @ProcDescricao = ERROR_MESSAGE();
			SELECT @ProcId AS Id, @ProcStatus AS Status, @ProcDescricao AS Descricao;
		END;

	END CATCH

END;

---
CREATE PROCEDURE [dbo].[SP_UsuarioAtualizar]
    @Id                   VARCHAR(11) = '',
    @DataNascimento       VARCHAR(11) = '',
    @Nome                 VARCHAR(50) = '',
    @SobreNome            VARCHAR(20) = '',
    @Email                VARCHAR(50) = '',
    @Telefone             VARCHAR(20) = '',
    @Perfil               VARCHAR(20) = '',
    @Status               VARCHAR(1) = 'N'
AS

BEGIN

	DECLARE @SetId INT = 0;
	DECLARE @ProcId VARCHAR(11) = '0';
	DECLARE @ProcStatus VARCHAR(3) = '';
	DECLARE @ProcDescricao VARCHAR(100) = '';

	BEGIN TRY

		UPDATE [dbo].[TB_Usuario] SET
			DataNascimento = @DataNascimento,
			Nome = @Nome,
			SobreNome = @SobreNome,
			Email = @Email,
			Telefone = @Telefone,
			Perfil = @Perfil,
			Status = @Status
		WHERE Id = @Id;

		UPDATE [dbo].[TB_Usuario_Login] SET
			Email = @Email,
			Status = @Status
		WHERE IdUsuario = @Id;

		SET @ProcId = @Id;
		SET @ProcStatus = '000';
		SET @ProcDescricao = 'Requisição efetuada com sucesso.';
		SELECT @ProcId AS Id, @ProcStatus AS Status, @ProcDescricao AS Descricao;

	END TRY
	BEGIN CATCH

		IF(@@TRANCOUNT > 0)
		BEGIN
			SET @ProcId = '0';
			SET @ProcStatus = CAST(@@TRANCOUNT AS VARCHAR(3));
			SET @ProcDescricao = ERROR_MESSAGE();
			SELECT @ProcId AS Id, @ProcStatus AS Status, @ProcDescricao AS Descricao;
		END;

	END CATCH

END;

---
CREATE PROCEDURE [dbo].[SP_UsuarioCadastrar]
    @DataCadastro         VARCHAR(24) = '',
    @DataNascimento       VARCHAR(11) = '',
    @Nome                 VARCHAR(50) = '',
    @SobreNome            VARCHAR(20) = '',
    @Email                VARCHAR(50) = '',
    @Telefone             VARCHAR(20) = '',
    @Perfil               VARCHAR(20) = '',
    @Password             VARCHAR(MAX) = '',
    @Status               VARCHAR(1) = 'N'
AS

BEGIN

	DECLARE @SetId INT = 0;
	DECLARE @ProcId INT = 0;
	DECLARE @ProcStatus VARCHAR(3) = '';
	DECLARE @ProcDescricao VARCHAR(100) = '';

	BEGIN TRY

		SELECT @SetId = ISNULL(Id, 0) FROM [dbo].[TB_Usuario] WHERE Email = @Email;

		IF(@SetId = 0)
			BEGIN
				INSERT INTO [dbo].[TB_Usuario] (
					DataCadastro, DataNascimento, Nome, SobreNome, Email, Telefone, Perfil, [Status]
				) VALUES (
					@DataCadastro,
					@DataNascimento,
					@Nome,
					@SobreNome,
					@Email,
					@Telefone,
					@Perfil,
					@Status
				);

				SET @ProcId = @@IDENTITY;
				SET @ProcStatus = '000';
				SET @ProcDescricao = 'Requisição efetuada com sucesso.';
				SELECT @ProcId AS Id, @ProcStatus AS Status, @ProcDescricao AS Descricao;

				IF(@ProcId > 0)
					BEGIN
						INSERT INTO [dbo].[TB_Usuario_Login] (
							IdUsuario, DataCadastro, Email, PasswordHash, [Status]
						) VALUES (
							@ProcId,
							@DataCadastro,
							@Email,
							@Password,
							@Status
						);
					END;

			END;
		ELSE
			BEGIN
				SET @ProcId = 0;
				SET @ProcStatus = '901';
				SET @ProcDescricao = 'O e-mail já foi utilizado em outro usuário.';
				SELECT @ProcId AS Id, @ProcStatus AS Status, @ProcDescricao AS Descricao;
			END;

	END TRY
	BEGIN CATCH

		IF(@@TRANCOUNT > 0)
		BEGIN
			SET @ProcId = 0;
			SET @ProcStatus = CAST(@@TRANCOUNT AS VARCHAR(3));
			SET @ProcDescricao = ERROR_MESSAGE();
			SELECT @ProcId AS Id, @ProcStatus AS Status, @ProcDescricao AS Descricao;
		END;

	END CATCH

END;

---
CREATE PROCEDURE [dbo].[SP_UsuarioListar]
    @Id                   VARCHAR(11) = '',
    @Nome                 VARCHAR(50) = '',
    @SobreNome            VARCHAR(20) = '',
    @Telefone             VARCHAR(20) = '',
    @Perfil               VARCHAR(20) = '',
    @Status               VARCHAR(1) = 'A'
AS

BEGIN

	DECLARE @SQL VARCHAR(MAX);
	DECLARE @ProcId VARCHAR(11) = '0';
	DECLARE @ProcStatus VARCHAR(3) = '';
	DECLARE @ProcDescricao VARCHAR(100) = '';

	BEGIN TRY

		SET @SQL = '';
		SET @SQL += 'SELECT * FROM [dbo].[TB_Usuario] WHERE Id > 0 ';
		IF(@Id <> '0') BEGIN SET @SQL += 'AND Id = ' + @Id + ' '; END;
		ELSE
			IF(@Nome <> '') BEGIN SET @SQL += 'AND Nome LIKE ''%' + @Nome + '%'' '; END;
			IF(@SobreNome <> '') BEGIN SET @SQL += 'AND SobreNome LIKE ''%' + @SobreNome + '%'' '; END;
			IF(@Telefone <> '') BEGIN SET @SQL += 'AND Telefone LIKE ''%' + @Telefone + '%'' '; END;
			IF(@Perfil <> '') BEGIN SET @SQL += 'AND Perfil LIKE ''%' + @Perfil + '%'' '; END;
			IF(@Status <> 'T') BEGIN SET @SQL += 'AND Status = ''' + @Status + ''' '; END;

		EXECUTE(@SQL);
		--PRINT(@SQL);

	END TRY
	BEGIN CATCH

		IF(@@TRANCOUNT > 0)
		BEGIN
			SET @ProcId = '0';
			SET @ProcStatus = @@TRANCOUNT;
			SET @ProcDescricao = ERROR_MESSAGE();
			SELECT @ProcId AS Id, @ProcStatus AS Status, @ProcDescricao AS Descricao;
		END;

	END CATCH

END;

---
CREATE PROCEDURE [dbo].[SP_UsuarioLogin]
    @Email       VARCHAR(50) = '',
    @Password    VARCHAR(MAX) = ''
AS

BEGIN

	DECLARE @SQL VARCHAR(MAX);
	DECLARE @Sel_IdUsuario INT = 0;
	DECLARE @ProcId VARCHAR(11) = '0';
	DECLARE @ProcStatus VARCHAR(3) = '';
	DECLARE @ProcDescricao VARCHAR(100) = '';

	BEGIN TRY

		SELECT TOP 1 @Sel_IdUsuario = ISNULL(IdUsuario, 0) FROM [dbo].[TB_Usuario_Login] 
		WHERE Id > 0
		AND Email = @Email
		AND PasswordHash = @Password
		AND [Status] IN ('N','A','P')
		ORDER BY Id DESC

		IF(@Sel_IdUsuario > 0)
			BEGIN
				SELECT TOP 1 * FROM [dbo].[TB_Usuario] 
				WHERE Id = @Sel_IdUsuario
				AND Email = @Email
				AND [Status] IN ('N','A','P')
			END;
		ELSE
			BEGIN
				SET @ProcId = '0';
				SET @ProcStatus = '901';
				SET @ProcDescricao = 'Ops!<br/>E-mail ou a senha, não são válidos.';
				SELECT @ProcId AS Id, @ProcStatus AS Status, @ProcDescricao AS Descricao;
			END;

	END TRY
	BEGIN CATCH

		IF(@@TRANCOUNT > 0)
		BEGIN
			SET @ProcId = '0';
			SET @ProcStatus = CAST(@@TRANCOUNT AS VARCHAR(3));
			SET @ProcDescricao = ERROR_MESSAGE();
			SELECT @ProcId AS Id, @ProcStatus AS Status, @ProcDescricao AS Descricao;
		END;

	END CATCH

END;
