<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Test.aspx.cs" Inherits="Treasure.Main.Test" %>

<!DOCTYPE html> 
<html> 
<head> 
<meta charset="utf-8" /> 
<title>大div中小div靠下实例 在线演示 DIVCSS5</title> 
<style> 
.divcss5{position:relative;width:400px;height:300px;border:1px solid #F00} 
.diva{position:absolute;width:50px;height:100px;bottom:0;left:120px;background:#00F} 
.divb{position:absolute;width:50px;height:150px;bottom:0;left:180px;background:#00F} 
</style> 
</head> 
<body> 

<div class="diva"></div> 
<div class="divb"></div> 

</body> 
</html> 
