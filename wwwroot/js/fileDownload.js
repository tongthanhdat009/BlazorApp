window.downloadFileFromStream = async (fileName, contentStreamReference) => {
  const arrayBuffer = await contentStreamReference.arrayBuffer();
  const blob = new Blob([arrayBuffer]);
  const url = URL.createObjectURL(blob);
  const anchorElement = document.createElement("a");
  anchorElement.href = url;
  anchorElement.download = fileName ?? "";
  anchorElement.click();
  anchorElement.remove();
  URL.revokeObjectURL(url);
};

window.downloadFileFromBytes = (fileName, base64String) => {
  const link = document.createElement("a");
  link.download = fileName;
  link.href = "data:application/pdf;base64," + base64String;
  document.body.appendChild(link);
  link.click();
  document.body.removeChild(link);
};
