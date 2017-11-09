<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GSEITAndCs_French.aspx.cs" Inherits="GCC_Web_Portal.Surveys.GSEI.GSEITAndCs_French" %>

<%@ Import Namespace="SharedClasses" %>
<!DOCTYPE html>
<html lang="en" class="no-js">
<head runat="Server">
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <title>Terms and Conditions</title>
    <asp:PlaceHolder runat="server">
        <%: Scripts.Render("~/bundles/modernizr") %>
    </asp:PlaceHolder>
    <webopt:BundleReference runat="server" Path="~/Content/bootstrap" />
    <webopt:BundleReference runat="server" Path="~/Content/css" />
    <!--[if lt IE 9]>
      <script src="https://oss.maxcdn.com/html5shiv/3.7.2/html5shiv.min.js"></script>
      <script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
    <![endif]-->
</head>
<body>
    <form runat="server">
        <div class="container">
            <div class="col-xs-12" id="main-content">
                <h1> Concours d'enquête sur le service à la clientèle </h1>
                <h3> Conditions d'utilisation </h3>
                <ul>
                    <li> Pour participer et être admissible à gagner, le participant doit répondre à toutes les questions obligatoires de l'Enquête sur l'expérience des invités. </li>
                    <li> Aucun achat requis. </li>
                    <li> Le participant sélectionné doit avoir respecté toutes les conditions du concours et répondre correctement à une question d'habileté mathématique pour gagner un prix. </li>
                    <li> Tous les formulaires d'inscription non gagnants, disqualifiés et / ou gagnants ainsi que les renseignements connexes seront la propriété de la Great Canadian Gaming Corporation (GCGC) ou de Great American Gaming. </li>
                    <li> GCGC se réserve le droit d'annuler ou de suspendre ce concours si des virus, des bugs ou d'autres causes indépendantes de leur volonté corrompent l'administration, la sécurité ou le jeu approprié de ce concours. </li>
                    <li> Si, pour quelque raison que ce soit, le Concours ne soit pas en mesure de fonctionner comme prévu ou si l'administration, la sécurité, l'équité, l'intégrité ou la bonne conduite de ce concours sont corrompus ou affectés par une infection de raison de virus informatique, de bogues, de falsification, D'intervention, de fraude, de défaillance technique ou de toute autre cause indépendante de sa volonté, GCGC se réserve le droit d'annuler, de résilier, de modifier ou de suspendre la partie électronique du concours, y compris l'annulation de toute méthode d'inscription. </li>
                    <li> GCGC peut, à sa seule discrétion, modifier les Conditions du Concours à tout moment. Les décisions du GCGC sont définitives. </li>
                    <li> GCGC n'est pas responsable des entrées perdues, mal adressées, mal placées, incomplètes, illisibles ou endommagées, des transmissions informatiques tronquées ou retardées, ou des entrées soumises après la date limite du concours. </li>
                    <li> GCGC n'est pas responsable des pannes téléphoniques, techniques, réseau, en ligne, électroniques, matérielles ou logicielles, ni de la congestion sur le site Web de l'entreprise. </li>
                    <li> En rendant le prix disponible pour un gagnant, GCGC ne fait aucune représentation ni aucune garantie expresse ou implicite, orale ou écrite, à l'égard du prix. </li>
                    <li> GCGC se réserve le droit de remplacer le prix ou une partie de celui-ci par un prix égal ou supérieur. </li>
                    <li> GCGC ne sera pas responsable envers le gagnant ou toute autre personne pour la perte ou les dommages causés à une personne, à un bien ou à un ordinateur résultant de la participation à ce concours ou lié à la participation à ce concours, au téléchargement de tout matériel relatif à ce concours, De GCGC et / ou le fournisseur du prix. </li>
                    <li> GCGC a le droit de publier le nom et l'emplacement du gagnant sans rémunération. </li>
                </ ul>
                <h3> <% = CasinoName%> Conditions du concours d'enquête sur le service à la clientèle </h3>
                <h4> Entrée qualifiée et participant </h4>
                <ul>
                    <li> Un participant admissible (participant) est une personne âgée de 19 ans et plus et un résident canadien. Les employés de la Great Canadian Gaming Corporation, de ses filiales et de ses sociétés affiliées, collectivement appelés «GCGC» British Columbia Lottery Corporation (BCLC), ainsi que les membres de leur famille et de leur ménage qui résident à la même adresse ne sont pas admissibles. </li>
                    <li> Les employés de Forum Research, de ses filiales et de ses sociétés affiliées ainsi que des membres de la famille et des membres du même ménage qui résident à la même adresse ne sont pas admissibles. </li>
                    <li> Les personnes volontairement exclues et interdites ne sont pas admissibles à participer et ne recevront pas de prix. </li>
                    <li> Le participant et l'entrée doivent satisfaire à toutes les conditions du concours, comme indiqué ci-dessous. L'entrée et / ou le participant qui ne répond pas aux conditions du concours seront disqualifiés et le prix sera perdu. </li>
                </ ul>
                <h4> Période du concours </h4>
                <p> La période du concours commence à partir du moment où le lien de l'Enquête sur l'expérience des invités est diffusé au public jusqu'au 22 janvier 2017 ou à toute autre date et heure affichée par GCGC.</p>
                <h4> Dossier de tirage </h4>
                <ul>
                  <li> Pour entrer et être admissible à gagner, le participant doit répondre à toutes les questions obligatoires sur le River Rock Casino Resort Guest Experience enquête. </li>
                    <li> Les inscriptions doivent être faites directement sur le site du sondage fourni via un lien dans un courriel envoyé par GCGC. Les photocopies, les télécopies, les courriels ou les appels téléphoniques ne sont pas considérés comme des inscriptions admissibles. </li>
                    <li> Les participants doivent fournir une adresse e-mail valide. Le nom et prénom de tous les participants en ligne établit l'identité du participant. </li>
                    <li> Le nom et l'adresse électronique de tous les participants seront utilisés pour effectuer le tirage. </li>
                    <li> Un participant ne peut participer au concours qu'une seule fois pendant la période promotionnelle. Les entrées en double seront supprimées. </li>
                    <li> L'utilisation de logiciels ou d'autres matériels à des fins de création de plusieurs entrées est interdite et entraînera la disqualification. </li>
                    <li> Toutes les entrées de toutes les propriétés GCGC en Colombie-Britannique seront regroupées chaque mois aux fins de la tenue du concours. </li>
                    <li> Onze (11) gagnants seront choisis au hasard parmi les participants à 13 h le 31 janvier ou dès que possible par la suite. Le processus de tirage est le suivant: </li>
                    <li> Toutes les participations seront collectées pendant la Période du Concours. </li>
                    <li> Lors de la soumission, chaque entrée sera affectée d'un nombre fractionnaire généré au hasard entre 0 et 1. Par exemple, 0.212874. </li>
                    <li> À la fin du mois, les entrées contenant les onze (11) plus grands nombres générés au hasard seront sélectionnées pour déterminer les gagnants. </li>
                    <li> Les nombres sont arbitraires et sont générés par un nombre de graines aléatoire. </li>
                    <li> La génération de nombres aléatoires n'est en aucune façon influencée par un employé de GCGC. </li>
                    <li> Les informations recueillies en ligne seront stockées dans une base de données sécurisée accessible uniquement par GCGC Marketing et Forum Research. Ces informations ne seront ni vendues ni copiées à d'autres tiers. </li>
                    <li> Les chances de gagner dépendront du nombre d'inscriptions admissibles reçues avant la date limite du concours. </li>
                    <li> Les participants gagnants seront contactés le plus tôt possible après le tirage au sort par GCGC par courrier électronique. </li>
                    <li> Les participants gagnants auront dix (10) jours pour réclamer le prix, après quoi le prix est confisqué et devient la propriété de GCGC. </li>
                    <li> Le gagnant recevra une (1) carte-cadeau d'une valeur de 1 000 $ et valide pour le rachat dans un grand casino canadien </li>
                    <li> Les 10 participants gagnants recevront chacun une (1) carte-cadeau d'une valeur de 100 $ et valide pour le rachat dans n'importe quel grand casino canadien </li>
                    <li> Il n'y a pas d'équivalent de trésorerie pour un prix. Tous les prix doivent être acceptés comme attribués. </li>
                    <li> Le gagnant doit réclamer le prix au bureau des Service à la clientèle du grand casino canadien de son choix. </li>
                    <li> Le gagnant est responsable de la collecte de son propre prix. </li>
                    <li> Le gagnant devra présenter une pièce d'identité avec photo valide (y compris une preuve d'âge) et remplir le formulaire d'autorisation du gagnant avant la remise du prix. Le non-respect de cette règle entraînera la disqualification du gagnant et le prix deviendra la propriété de GCGC. </li>
                    <li> Toute intention de la part du Participant de se déformer par l'utilisation d'alias et d'adresses de courriel sera disqualifiée. En cas de litige concernant l'identité de la personne qui soumet une inscription électronique, l'inscription sera considérée comme présentée par la personne dont le nom figure dans la base de données. </li>
                    <li> Si un participant est jugé inadmissible, un gagnant alternatif sera sélectionné dans le bassin de toutes les participations admissibles reçues. Les informations recueillies par GCGC seront stockées dans une base de données sécurisée accessible uniquement par le personnel du siège de GCGC. </li>
                </ ul>
                <h3> Informations personnelles et politique de confidentialité </h3>
                <p> Vos renseignements personnels sont recueillis et utilisés par GCGC conformément à la Loi sur l'accès à l'information et la protection de la vie privée de la Colombie-Britannique. Il sera utilisé à des fins de recherche GCGC et d'administrer ce concours. Vos informations ne seront pas vendues, partagées avec des tiers ou utilisé à des fins de sollicitation. Si vous avez des questions à ce sujet, veuillez écrire à l'agent de confidentialité de GCGC au 95, rue Schooner, Coquitlam, C.-B. V3K 7A8. </p>
                    </div>


            </div>
      
        <asp:ScriptManager runat="server">
            <Scripts>
                <%--To learn more about bundling scripts in ScriptManager see http://go.microsoft.com/fwlink/?LinkID=272931&clcid=0x409 --%>
                <%--Framework Scripts--%>

                <asp:ScriptReference Name="MsAjaxBundle" />
                <asp:ScriptReference Name="jquery" />
                <%--<asp:ScriptReference Name="jquery.ui.combined" />--%>
                <asp:ScriptReference Name="WebForms.js" Assembly="System.Web" Path="~/Scripts/WebForms/WebForms.js" />
                <asp:ScriptReference Name="WebUIValidation.js" Assembly="System.Web" Path="~/Scripts/WebForms/WebUIValidation.js" />
                <asp:ScriptReference Name="MenuStandards.js" Assembly="System.Web" Path="~/Scripts/WebForms/MenuStandards.js" />
                <asp:ScriptReference Name="GridView.js" Assembly="System.Web" Path="~/Scripts/WebForms/GridView.js" />
                <asp:ScriptReference Name="DetailsView.js" Assembly="System.Web" Path="~/Scripts/WebForms/DetailsView.js" />
                <asp:ScriptReference Name="TreeView.js" Assembly="System.Web" Path="~/Scripts/WebForms/TreeView.js" />
                <asp:ScriptReference Name="WebParts.js" Assembly="System.Web" Path="~/Scripts/WebForms/WebParts.js" />
                <asp:ScriptReference Name="Focus.js" Assembly="System.Web" Path="~/Scripts/WebForms/Focus.js" />
                <asp:ScriptReference Name="WebFormsBundle" />
                <%--Site Scripts--%>
            </Scripts>
        </asp:ScriptManager>
        <%: Scripts.Render("~/bundles/bootstrap") %>
    </form>
</body>
</html>
