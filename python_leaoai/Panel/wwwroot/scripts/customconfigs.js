////CKEDITOR.editorConfig = function (config) {
////    config.removeDialogTabs = 'image:advanced;image:Link;link:advanced;link:upload';
////    config.filebrowserImageUploadUrl = '/Imagem/CKImage' //Action for Uploding image
////};



CKEDITOR.editorConfig = function (config) {

	config.toolbarGroups = [
		{ name: 'clipboard', groups: ['clipboard', 'undo'] },
		{ name: 'editing', groups: ['find', 'selection', 'spellchecker', 'editing'] },
		{ name: 'links', groups: ['links'] },
		{ name: 'insert', groups: ['insert', 'Smiley'] },
		{ name: 'forms', groups: ['forms'] },
		{ name: 'tools', groups: ['tools'] },
		{ name: 'document', groups: ['mode', 'document', 'doctools'] },
		{ name: 'others', groups: ['others'] },
		'/',
		{ name: 'basicstyles', groups: ['basicstyles', 'cleanup'] },
		{ name: 'paragraph', groups: ['list', 'indent', 'blocks', 'align', 'bidi', 'paragraph'] },
		{ name: 'styles', groups: ['styles'] },
		{ name: 'colors', groups: ['colors'] },
		{ name: 'about', groups: ['about', 'VideoDetector'] }
	];

	config.removeButtons = 'Flash,Anchor,Language,Form,Checkbox,Textarea,TextField,Select,Button,ImageButton,HiddenField,SelectAll,Replace,Save,NewPage,Print,Templates,Radio,Subscript,Superscript,CopyFormatting,RemoveFormat,Styles,Font,BGColor';

	config.extraPlugins = 'videodetector';

	// Set the most common block elements.
	config.format_tags = 'p;h1;h2;h3;pre';

	// Simplify the dialog windows.
	//config.removeDialogTabs = 'image:advanced;link:advanced';
	config.removeDialogTabs = 'image:advanced;image:Link;link:advanced;link:upload';
	config.filebrowserImageUploadUrl = '/Imagem/CKImage'; //Action for Uploding image
	//config.enterMode = CKEDITOR.ENTER_BR;
};