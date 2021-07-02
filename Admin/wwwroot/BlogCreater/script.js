function add(type = "", data = "",des = "") {
	if (type == "")
		type = document.getElementById("ZATtype").value
    switch (type){
	case "p":
		addCard(generateText(data),"p",des);
		break;
	case "h":
			addCard(generateHeader(data), "h", des);
		break;
	case "hr":
			addCard(generateLine(data), "hr", des);
		break;
	case "img":
			addCard(generateImage(data), "img", des);
		break;
	case "video":
			addCard(generateVideo(data), "video", des);
		break;
	case "audio":
			addCard(generateAudio(data), "audio", des);
		break;
	case "html":
			addCard(generateHtml(data), "html", des);
		break;
	case "list":
		addCard(generateList(data),"list",des);
		break;
	}
}

var cDiv = document.getElementById("ATcards");
var context = [];
function createBlogContext(){
	fixElement("ATcards");
	context = [];
	if(bdrag){
		dragButton();
	}
	
	var xdata;
	for(i = 0; i < cDiv.children.length; i++){
		var kisa = cDiv.children[i].children[0];
		generateForBlogJson(kisa, cDiv.children[i].getAttribute("typeat"), context);
		//generateForIframeBlog(kisa, cDiv.children[i].getAttribute("typeat"), context);
	}
	blogJsonD.blogContent = context.toStringAT();

	return updateIframeBody(context.toStringAT());
	//updateIframeBody(context.join("  "));
}

Array.prototype.toStringAT = function() {
	return "[" + this.toString() + "]";
};

function generateForBlogJson(kisa, typeat, context) {//Eski sistem bu
	var xdataT;
    var xdataDes = " ";
	switch(cDiv.children[i].getAttribute("typeat")){
		case "p":
			xdataT = "p";
			xdata = kisa.getElementsByTagName("textarea")[0].innerHTML;
            xdataDes = cDiv.children[i].getAttribute("des");
			break;
		case "h":
			xdataT = "h";
			xdata = kisa.getElementsByTagName("input")[3].value;
            xdataDes = cDiv.children[i].getAttribute("des");
			//xdata = "<p class='h" + kisa.getElementsByTagName("select")[0].value + ((typeof(kisa.getElementsByTagName("input")[1].getAttribute("checked")) == "string") ? " center": "") + "'>" + kisa.getElementsByTagName("input")[2].value + "</p>";
			break;
		case "hr":
			xdataT = "hr";
			xdata = "";
            xdataDes = cDiv.children[i].getAttribute("des");
			//xdata = "<hr>";
			break;
		case "img":
			xdataT = "img";
			xdata = kisa.getElementsByTagName("input")[2].value;
            xdataDes = cDiv.children[i].getAttribute("des");
			//xdata = "<img src='" + kisa.getElementsByTagName("input")[1].value  + "' alt='" + kisa.getElementsByTagName("input")[1].value + "'>";
			break;
		case "video":
			alert("Daha Bitmedi!!! bu cıktı bozuk olabilir");
			//xdata = "<video controls> <source src='" + kisa.getElementsByTagName("input")[1].value + "' type='video/mp4'> </video>";
			break;
		case "audio":
			alert("Daha Bitmedi!!! bu cıktı bozuk olabilir");
			//xdata = "<audio controls> <source src='" + kisa.getElementsByTagName("input")[1].value + "' type='video/mp4'> </audio>";//////////////////////////////
			break;
		case "html":
			alert("Daha Bitmedi!!! bu cıktı bozuk olabilir");
			//xdata = kisa.getElementsByTagName("textarea")[0].innerHTML;
			break;
		case "list":
			alert("Daha Bitmedi!!! bu cıktı bozuk olabilir");
			//xdata = alert("LİST DAHA HAZIR DEĞİL");
			break;
	}

	context.push(JSON.stringify({ type: xdataT, data: xdata, description: xdataDes }));
	return xdata;
}

function generateForIframeBlog(kisa, typeat, context) {//Eski sistem bu -----  des eksik 
		switch(cDiv.children[i].getAttribute("typeat")){
			case "p":
				xdata = "<p>" + kisa.getElementsByTagName("textarea")[0].innerHTML + "</p>";
				break;
			case "h":
				xdata = "<p class='h" + kisa.getElementsByTagName("select")[0].value + ((typeof(kisa.getElementsByTagName("input")[1].getAttribute("checked")) == "string") ? " center": "") + "'>" + kisa.getElementsByTagName("input")[2].value + "</p>";
				break;
			case "hr":
				xdata = "<hr>";
				break;
			case "img":
				xdata = "<img src='" + kisa.getElementsByTagName("input")[1].value  + "' alt='" + kisa.getElementsByTagName("input")[1].value + "'>";
				break;
			case "video":
				xdata = "<video controls> <source src='" + kisa.getElementsByTagName("input")[1].value + "' type='video/mp4'> </video>";
				break;
			case "audio":
				xdata = "<audio controls> <source src='" + kisa.getElementsByTagName("input")[1].value + "' type='video/mp4'> </audio>";//////////////////////////////
				break;
			case "html":
				xdata = kisa.getElementsByTagName("textarea")[0].innerHTML;
				break;
			case "list":
				xdata = alert("LİST DAHA HAZIR DEĞİL");
				break;
		}
		context.push(xdata);
		return xdata;
}

function addCard(card,typeAT,des=""){
	//var type = document.getElementById("ZATtype").value;
	//var cDiv = document.getElementById("ATcards");
	var addede = document.createElement("div");
	addede.setAttribute("typeat", typeAT);
	addede.setAttribute("des", des);
	addede.innerHTML = card;
	addede.setAttribute("class",'board-item');
	cDiv.appendChild(addede);
	columnGrids[0].add([addede]);
	
	//cDiv.innerHTML += addede; 
	//anan(true);
}

function addDes(element) {
	var outputDe = prompt("Acıklamayı ayarlayın", element.parentElement.parentElement.getAttribute("des"));
    if (outputDe == null)
        outputDe = "";
	element.parentElement.parentElement.setAttribute("des", outputDe);
}

function deleteCard(element){
	element.parentElement.parentElement.outerHTML = "";
}
function generateDelete(){
    return "<input type='submit' onclick='deleteCard(this)' value='Delete' class='ZbuttonDelete'>";
}
function generateAddDes(){
	return "<input type='submit' onclick='addDes(this)' value='AcıklamaEkle' class='ZbuttonDelete' style='background: #4193ff;'>";
}
function generateElement(elementAt){
	return "<div class='board-item-content'>" + elementAt + "</div>";
	//return "<div class='board-item'><div class='board-item-content'>"+elementAt+"</div></div>";
}
function generateLabel(textL){
	return "<label>" + textL + "</label>" + generateDelete() + generateAddDes();
}


function generateText(data = ""){
	return generateElement(generateLabel("Text") + "<br/><textarea style='width: 95%'>" + data + "</textarea>");
}
function generateHeader(data = ""){
	return generateElement(generateLabel("Header") + "<br/><select><option value='1'>1</option><option value='2'>2</option><option value='3'>3</option><option value='4'>4</option><option value='5'>5</option><option value='6'>6</option></select>    <input type='checkbox' checked='true'>Center</input>     <br/><input style='width: 95%' type='text' value='" + data + "'>");
}
function generateLine(data = ""){
	return generateElement(generateLabel("Line") + "<br/><hr>");
}
function generateImage(data = ""){
	return generateElement(generateLabel("Image") + "<br/><input style='width: 95%' type='text' value='" + data + "' oninput='imgChange(this)'>   <br/>   <img src='" + data + "' alt='Image' width='95%' height='auto'>");
}
function generateVideo(data = ""){
	return generateElement(generateLabel("Video") + "<br/><input style='width: 95%' type='text' value='" + data + "' oninput='videoChange(this)'>   <br/>   <video width='95%' height='auto' controls>  <source src='" + data + "' type='video/mp4'> </video>");
}
function generateAudio(data = ""){
	return generateElement(generateLabel("Audio") + "<br/><input style='width: 95%' type='text' value='" + data + "' oninput='audioChange(this)'>   <br/>   <audio width='95%' height='auto' controls>  <source src='" + data + "' type='audio/mpeg'> </audio>");
}
function generateHtml(data = ""){
	return generateElement(generateLabel("Html") + "<b style='color:red'>  ADMİNLER İÇİN</b><br/><textarea style='width: 95%'>" + data + "</textarea>");
}
function generateList(data = "") {
    return generateElement(generateLabel("List") + "<select oninput='alert(this)'><option value='ol'>Sıralı</option><option value='ul'>Sırasız</option></select>   <b style='color:red'>  YAPIM ASAMASINDA</b>   <br/><input style='width: 90%' type='text' value='Url' oninput='(this)'> <input style='width: 5%' type='button' value='+' oninput='(this)'>");
}

function audioChange(element){
	var textDiv = element.parentElement.getElementsByTagName('input');
	
	for(i = 0; i < textDiv.length; i++){
		if(textDiv[i].getAttribute("type") == "text"){
			var audio = element.parentElement.getElementsByTagName('audio')[0];
			audio.getElementsByTagName('source')[0].setAttribute('src',textDiv[i].value);
			audio.load();
			textDiv[i].setAttribute("value",textDiv[i].value);
		}
	}
	
}
function videoChange(element){
	var textDiv = element.parentElement.getElementsByTagName('input');
	
	for(i = 0; i < textDiv.length; i++){
		if(textDiv[i].getAttribute("type") == "text"){
			var video = element.parentElement.getElementsByTagName('video')[0];
			video.getElementsByTagName('source')[0].setAttribute('src',textDiv[i].value);
			video.load();
			textDiv[i].setAttribute("value",textDiv[i].value);
		}
	}
	
}
function imgChange(element){
	var textDiv = element.parentElement.getElementsByTagName('input');
	
	for(i = 0; i < textDiv.length; i++){
		if(textDiv[i].getAttribute("type") == "text"){
			element.parentElement.getElementsByTagName('img')[0].setAttribute('src',textDiv[i].value);
			textDiv[i].setAttribute("value",textDiv[i].value);
		}
	}
	
}
function dragEnabled(tf){
	if(!tf){
		ccss(true);
		fixCard("ATcards");
		//fixCard("ATtest");
		for(i = 0;i<columnGrids.length;i++){
			columnGrids[i].destroy();
		}
	}
	else{
		ccss(false);
		anan(true);
	}
}


function ccss(tf){
	var cssL = document.getElementById("cssLink");
	if(tf/*cssL.getAttribute("href") == "dragon.css"*/){
		cssL.setAttribute("href", "/BlogCreater/dragoff.css");
	}
	else{
		cssL.setAttribute("href", "/BlogCreater/dragon.css");
	}
}

function dragCheckbox(){
	if(document.getElementById("checkboxAt").checked == true)
	{
		dragEnabled(true);
		document.getElementById("checkboxAt").checked == true;
	}
	else{
		dragEnabled(false);
		document.getElementById("checkboxAt").checked == false;
	}
}

var bdrag = true;
function dragButton(){
	if(bdrag == true)
	{
		bdrag = false;
		dragEnabled(false);
	}
	else{
		bdrag = true;
		dragEnabled(true);
	}
}

function fixCard(divId){
	fixElement(divId);
	
	var fcDiv = document.getElementById(divId);
	var a,b,c,d,f,p,e = 9999;
	var s=[];
	var o=[];
	var y=[];
	var t=[];
	for(i = 0; i < fcDiv.children.length; i++){
		//a = fcDiv.children[0].getAttribute("style").toString();
		a = fcDiv.children[i].style.cssText.toString();
		c = a.search("teY");
		b = a.slice(c+4,c+4+5).toString();
		d = parseInt(b.replace(")", "").replace("x", "").replace("p", "").replace(";", ""));
		s.push(d);
	}
	for(i = 0; i < s.length; i++){
		o.push(s[i]);
	}
	for(i = 0; i < fcDiv.children.length; i++){
		y.push(fcDiv.children[i].outerHTML);
	}
	var boll= true;
	while(boll){
		for(i = 0; i < o.length; i++){
			if(e > o[i]){
				e = o[i];
				p = i;
			}
		}
		t.push(y[p]);
		kes(y,y[p]);
		kes(o,e);
		e = 9999;
		if(o.length == 0){
			boll = false;
		}
	}
	fcDiv.innerHTML = t.join("  ");
	for(i = 0; i < fcDiv.children.length; i++){
		fcDiv.children[i].setAttribute("style","board-item");
	}
}
function kes(a,b){
	var index = a.indexOf(b);
	if (index > -1) {
		a.splice(index, 1);
	}
}

function fixElement(textDiv){
	fixTextarea(textDiv);
	fixInput(textDiv);
	fixSelect(textDiv);
}
function fixTextarea(textDiv){
	var fta = document.getElementById(textDiv).getElementsByTagName("textarea");
	
	for(i = 0; i < fta.length; i++){
		fta[i].innerHTML = fta[i].value;
	}
}
function fixInput(textDiv){
	var fi = document.getElementById(textDiv).getElementsByTagName("input");
	
	for(i = 0; i < fi.length; i++){
		switch(fi[i].getAttribute("type")){
			case "checkbox":
				if(fi[i].checked){
					fi[i].setAttribute("checked","");
				}
				else{
					fi[i].removeAttribute("checked");
				}
				break;
			
			case "text":
				fi[i].setAttribute("value", fi[i].value);
				break;
		}
	}
}
function fixSelect(textDiv){
	var fs = document.getElementById(textDiv).getElementsByTagName("select");
	
	for(i = 0; i < fs.length; i++){
		var patater = fs[i].value;
		for(j = 0; j < fs[i].length; j++){
			if(fs[i][j].value == patater){
				fs[i][j].setAttribute("selected","");
			}
			else fs[i][j].removeAttribute("selected");
		}
		//fs[i][patater-1].setAttribute("selected","");
	}
}

var aar;
function updateIframeBody(text) {
	aar = text;
	console.log(text);
	document.getElementById("ZtestAT").innerHTML = text;
    return aar;
    //var myFrame = document.getElementById("iframeAT");
    //var myFramey = (myFrame.contentWindow || myFrame.contentDocument);
    //if (myFramey.document)myFramey = myFramey.document;
    //var myFrame = document.getElementById("iframeAT").contents().find('body');
    //myFramey.head.innerHTML = "<link rel='stylesheet' type='text/css' href='atduyarStyle.css'>";
    //myFramey.body.innerHTML = text;
}





function getBlogData(dataJsonString) {
	json = JSON.parse(dataJsonString);
    document.getElementById("ATcards").innerHTML = "";//tüm car ları siler :D
	document.getElementById("ZtestAT").innerHTML = "";//tüm car ları siler :D
    for (var i = 0; i < json.length; i++) {
		add(json[i].type, json[i].data, json[i].description);
	}
	dragButton();
	dragButton();
    createBlogContext();
}


  





//-------------------------------------------------------------------------------------------------


var dragContainer = document.querySelector('.drag-container');
var itemContainers = [].slice.call(document.querySelectorAll('.board-column-content'));
var columnGrids = [];
var boardGrid;

anan(true);
function anan(tf){
delete grid
dragContainer = document.querySelector('.drag-container');
itemContainers = [].slice.call(document.querySelectorAll('.board-column-content'));
columnGrids = [];
boardGrid;

// Init the column grids so we can drag those items around.
itemContainers.forEach(function (container) {
	var grid = new Muuri(container, {
    items: '.board-item',
    dragEnabled: tf,
    dragSort: function () {
      return columnGrids;
    },
    dragContainer: dragContainer,
    dragAutoScroll: {
      targets: (item) => {
        return [
          { element: window, priority: 0 },
          { element: item.getGrid().getElement().parentNode, priority: 1 },
        ];
      }
    },
  })
  .on('dragInit', function (item) {
    item.getElement().style.width = item.getWidth() + 'px';
    item.getElement().style.height = item.getHeight() + 'px';
  })
  .on('dragReleaseEnd', function (item) {
    item.getElement().style.width = '';
    item.getElement().style.height = '';
    item.getGrid().refreshItems([item]);
  })
  .on('layoutStart', function () {
    boardGrid.refreshItems().layout();
  });

  columnGrids.push(grid);
});


// Init board grid so we can drag those columns around.
boardGrid = new Muuri('.board', {
  dragEnabled: tf,
  dragHandle: '.board-column-header'
});

}

