#pragma once

#include <ShObjIdl_core.h>

class VkloudMenuExtention
    : public IShellExtInit
    , public IContextMenu
{
public:
    VkloudMenuExtention();

    IFACEMETHODIMP QueryInterface(REFIID riid, void** ppv) override;
    IFACEMETHODIMP_(ULONG) AddRef() override;
    IFACEMETHODIMP_(ULONG) Release() override;

    IFACEMETHODIMP Initialize(LPCITEMIDLIST pidlFolder, LPDATAOBJECT dataObject, HKEY key) override;

    IFACEMETHODIMP QueryContextMenu(HMENU menu, UINT indexMenu, UINT idCmdFirst, UINT idCmdLast, UINT flags) override;
    IFACEMETHODIMP InvokeCommand(LPCMINVOKECOMMANDINFO info) override;
    IFACEMETHODIMP GetCommandString(UINT_PTR isCommand, UINT flags, UINT* reserved, LPSTR name, UINT cchMax) override;

private:
    long refCounter;
    PCWSTR menuText;
};