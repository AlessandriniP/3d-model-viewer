const basePath = 'dist/3d-model-viewer/browser'
const { app, BrowserWindow } = require('electron/main')

function createWindow() {
  const win = new BrowserWindow({
    width: 1280,
    height: 720,
    minWidth: 400,
    minHeight: 500,
    icon: basePath + '/favicon.ico',
  })

  win.removeMenu()
  //win.webContents.openDevTools()
  win.loadFile(basePath + '/index.html')
}

app.whenReady().then(() => {
  createWindow()

  app.on('activate', () => {
    if (BrowserWindow.getAllWindows().length === 0) {
      createWindow()
    }
  })
})

app.on('window-all-closed', () => {
  if (process.platform !== 'darwin') {
    app.quit()
  }
})
