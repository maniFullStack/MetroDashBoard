<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TandCGTA_French.aspx.cs" Inherits="GCC_Web_Portal.TandSCTI_French" %>



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
                <h1>Programme de rétroaction des clients du Casino Woodbine  </h1>
                <h3>Conditions d'utilisation</h3>
                <ul>

<li> Pour participer et être admissible à gagner, le participant doit répondre à toutes les questions obligatoires du sondage sur l’expérience des clients.</li>
<li> Aucun achat n’est nécessaire. </li>
<li> Le participant sélectionné doit avoir respecté toutes les conditions du concours et avoir répondu correctement à une question d'habileté mathématique pour gagner un prix.</li>
<li> Tous les formulaires d'inscription non gagnants, disqualifiés ou gagnants ainsi que les renseignements connexes seront la propriété de la Great Canadian Gaming Corporation (GCGC).</li>
<li> La GCGC se réserve le droit d'annuler ou de suspendre ce concours si des virus, des bogues ou d'autres causes indépendantes de leur volonté corrompent l'administration, la sécurité ou le déroulement de ce concours.</li>
<li> Si, pour quelque raison que ce soit, le concours n'est pas en mesure de fonctionner comme prévu ou si l'administration, la sécurité, l'équité, l'intégrité ou la bonne conduite de ce concours est corrompue ou affectée par la raison d'infection de virus informatique, de fraude, de défaillance technique ou toute autre cause échappant à son contrôle, la GCGC se réserve le droit d'annuler, de résilier, de modifier ou de suspendre la partie électronique du concours, y compris l'annulation de toute méthode d'inscription.</li>
<li> La GCGC peut, à sa seule discrétion, modifier les conditions du concours à tout moment. Les décisions de la GCGC sont définitives.</li>
<li> La GCGC n'est pas responsable des inscriptions perdues, mal adressées, mal placées, incomplètes, illisibles ou endommagées, des transmissions informatiques tronquées ou retardées ou des inscriptions soumises après la date limite du concours.</li>
<li> La GCGC n'est pas responsable des pannes téléphoniques, techniques, de réseau, en ligne, électroniques, matérielles ou de logiciels, ni de la congestion sur le site Web de l'entreprise, de quelque nature que ce soit.</li>
<li> En rendant le prix disponible pour un participant gagnant, la GCGC ne fait aucune représentation ou garantie, que ce soit exprimée ou implicite, orale ou écrite, à l'égard du prix.</li>
<li> La GCGC se réserve le droit de substituer le prix ou une partie de celui-ci pour un prix égal ou supérieur.</li>
<li> La GCGC ne peut être tenue responsable envers le gagnant ou toute autre personne pour la perte ou les dommages causés à une personne, à un bien ou à un ordinateur résultant de la participation à ce concours ou lié à la participation à ce concours, au téléchargement de tout matériel relatif à ce concours ou aux actes ou omissions de la GCGC ou le fournisseur du prix.</li>
<li> La GCGC a le droit de publier le nom et l'emplacement du gagnant sans rémunération.</li>


                </ul>



                <h3>Conditions du concours du sondage sur l’expérience des clients du Casino Woodbine  </h3>
                <p>Pour le mois de  <%= DateTime.Now.ToString("MMMM yyyy",System.Globalization.CultureInfo.CreateSpecificCulture("fr-FR")) %></p>

                <h4>Inscription et participation admissible </h4>
                <ul>

                    <li>Un participant admissible est une personne âgée de 19 ans ou plus et un résident canadien. Les employés de la Great Canadian Gaming Corporation, de ses filiales et de ses sociétés affiliées, collectivement appelées « GCGC » et de la British Columbia Lottery Corporation (BCLC), ainsi que les membres de leur famille et de leur ménage qui résident à la même adresse ne sont pas admissibles.</li>
                    <li>Les employés de Forum Research, de ses filiales et sociétés affiliées, ainsi que les membres de la famille et les membres du même ménage qui résident à la même adresse ne sont pas admissibles.</li>
                    <li>Les personnes volontairement exclues et interdites ne sont pas admissibles à participer et ne recevront pas de prix.</li>
                    <li>Le participant et l’inscription doivent satisfaire à toutes les conditions du concours, comme il est indiqué ci-dessous. L’inscription ou le participant qui ne répond pas aux conditions du concours sera disqualifié et le prix sera annulé.</li>



                </ul>
                <h4>Durée du concours</h4>
                <p>La période du concours commence à partir du moment où le lien du sondage sur l’expérience des clients est diffusé au public jusqu'à la fin du même mois ou à toute autre date et heure affichée par la GCGC. Chaque mois civil sera considéré comme une période promotionnelle différente avec son propre bassin de participants et de prix. Les participants sont censés avoir Terminer le sondage et participé au concours dans le même mois qu’il a été rendu public. Les participants qui remplissent le sondage après la fin du mois au cours duquel le sondage a été publié seront automatiquement inscrits dans le groupe de sondage pour le mois au cours duquel ils ont réellement Terminer le sondage.</p>
                <h4>Tirage au sort</h4>
                <ul>


<li> Pour participer et être admissible à gagner, le participant doit répondre à toutes les questions obligatoires du sondage sur l’expérience des clients du Shorelines Casino.</li>
<li> Les inscriptions doivent être faites directement sur le site du sondage fourni via un lien dans un courriel envoyé par la GCGC. Les photocopies, les télécopies, les courriels ou les appels téléphoniques ne sont pas considérés comme des inscriptions admissibles.</li>
<li> Les participants doivent fournir une adresse électronique valide. Le prénom et nom de tous les participants en ligne établit l'identité du participant.</li>
<li> Le nom et l'adresse électronique de tous les participants seront utilisés pour effectuer le tirage au sort.</li>
<li> Un participant ne peut participer au concours qu'une seule fois pendant la période promotionnelle. Les entrées en double seront supprimées.</li>
<li> L'utilisation de logiciels ou d'autres matériels à des fins de saisie multiple est interdite et entraînera la disqualification.</li>
<li> Toutes les inscriptions provenant de toutes les propriétés de Shorelines Casino en Ontario seront regroupées chaque mois aux fins de la tenue du concours.</li>
<li> Un (1) gagnant sera choisi au hasard parmi les participants le 1er jour du mois suivant la réception de l’inscription, à 13 h ou dès que possible après 13 h. Le processus de tirage au sort est le suivant :</li>
<li> Toutes les inscriptions seront recueillies pendant la période du concours.</li>
<li> Lors de la soumission, chaque inscription recevra un nombre fractionnaire généré de façon aléatoire entre 0 et 1. Par exemple, 0,212874.</li>
<li> À la fin du mois, l’inscription avec le plus grand nombre généré au hasard sera sélectionnée.</li>
<li> Les nombres sont arbitraires et sont générés par un nombre de valeurs initiales aléatoires.</li>
<li> La génération de nombres aléatoires n'est en aucune façon influencée par un employé de la GCGC.</li>
<li> Les renseignements amassés en ligne seront enregistrés dans une base de données sécurisée accessible uniquement par GCGC Marketing et Forum Research. Ces renseignements ne seront ni vendus ni copiés pour une autre tierce partie.</li>
<li> Les chances de gagner dépendront du nombre d'inscriptions admissibles reçues avant la date limite du concours.</li>
<li> Les participants gagnants seront contactés par courriel le plus tôt possible après le tirage au sort par la GCGC.</li>
<li> Les participants gagnants auront dix (10) jours pour réclamer leur prix, après quoi le prix est annulé et devient la propriété de la GCGC.</li>
<li> Le gagnant recevra une (1) carte-cadeau d'une valeur de 100 $ pouvant être utilisée dans tout établissement de Shorelines Casino.</li>
<li> Aucun prix n’a une valeur monétaire. Tous les prix doivent être acceptés comme tels. </li>
<li> Le gagnant doit réclamer son prix au bureau du service à la clientèle du Shorelines Casino de son choix.</li>
<li> Le gagnant est responsable de ramasser son propre prix.</li>
<li> Le gagnant devra présenter une pièce d'identité avec photo valide (y compris une preuve d'âge) et remplir le formulaire d'autorisation du gagnant avant la remise du prix. Le non-respect de cette règle entraînera la disqualification du gagnant, et le prix deviendra la propriété de la GCGC.</li>
<li> Toute intention de la part du participant de se déformer par l'utilisation d'alias et d'adresses électroniques sera disqualifiée. En cas de litige concernant l'identité de la personne qui soumet une inscription électronique, l'inscription sera jugée comme ayant été  soumise par la personne dont le nom figure dans la base de données.</li>
<li> Si un participant est jugé inadmissible, un autre gagnant sera sélectionné dans le bassin de toutes les inscriptions admissibles reçues. Les renseignements recueillis par la GCGC seront enregistrés dans une base de données sécurisée accessible uniquement par le personnel du siège social de la GCGC.</li>



                </ul>
                <h3>Renseignements personnels et politique de confidentialité</h3>
                <p>Vos renseignements personnels sont recueillis et utilisés par le Casino Shorelines au nom de la Great Canadian Gaming Corporation (GCGC) et de la Société des loteries et des jeux de l'Ontario (OLG) conformément à la Loi sur l'accès à l'information et la protection de la vie privée. Vos renseignements personnels sont utilisés aux fins d’administrer ce concours, à la recherche du service à la clientèle et pour la recherche du jeu responsable. Ils seront utilisés à des fins de recherche pour la GCGC et pour administrer ce concours. Vos informations ne seront pas vendues, partagées avec des tiers ou utilisées à des fins de sollicitation. Si vous aviez des questions à ce sujet, veuillez écrire à l'agent de confidentialité de la GCGC au 95 Schooner Street, Coquitlam, C.B. V3K 7A8.</p>
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

