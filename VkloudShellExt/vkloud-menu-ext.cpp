#include "pch.h"
#include "vkloud-menu-ext.h"
//#include <Shlwapi.h>
//#include <strsafe.h>


#define IDM_DISPLAY 0

VkloudMenuExtention::VkloudMenuExtention()
    : refCounter{ 1 }
    , menuText{ L"Test menu" }
{
}

IFACEMETHODIMP VkloudMenuExtention::QueryInterface(REFIID riid, void** ppv)
{
    HRESULT result = S_OK;

    if (IsEqualIID(IID_IUnknown, riid) || IsEqualIID(IID_IContextMenu, riid))
    {
        *ppv = static_cast<IContextMenu*>(this);
    }
    else if (IsEqualIID(IID_IShellExtInit, riid))
    {
        *ppv = static_cast<IShellExtInit*>(this);
    }
    else
    {
        result = E_NOINTERFACE;
        *ppv = nullptr;
    }

    if (*ppv)
    {
        AddRef();
    }

    return result;
}

IFACEMETHODIMP_(ULONG) VkloudMenuExtention::AddRef()
{
    return InterlockedIncrement(&refCounter);
}

IFACEMETHODIMP_(ULONG) VkloudMenuExtention::Release()
{
    const auto ref = InterlockedDecrement(&refCounter);
    if (refCounter == 0)
    {
        delete this;
    }

    return ref;
}

IFACEMETHODIMP VkloudMenuExtention::Initialize(LPCITEMIDLIST idlFolder, LPDATAOBJECT dataObject, HKEY key)
{
    return S_OK;

    /*if (NULL == dataObject)
    {
        return E_INVALIDARG;
    }

    HRESULT hr = E_FAIL;

    FORMATETC fe = { CF_HDROP, NULL, DVASPECT_CONTENT, -1, TYMED_HGLOBAL };
    STGMEDIUM stm;

    // The pDataObj pointer contains the objects being acted upon. In this 
    // example, we get an HDROP handle for enumerating the selected files and 
    // folders.
    if (SUCCEEDED(dataObject->GetData(&fe, &stm)))
    {
        // Get an HDROP handle.
        HDROP hDrop = static_cast<HDROP>(GlobalLock(stm.hGlobal));
        if (hDrop != NULL)
        {
            // Determine how many files are involved in this operation. This 
            // code sample displays the custom context menu item when only 
            // one file is selected. 
            UINT nFiles = DragQueryFile(hDrop, 0xFFFFFFFF, NULL, 0);
            if (nFiles == 1)
            {
                // Get the path of the file.
                if (0 != DragQueryFile(hDrop, 0, m_szSelectedFile, ARRAYSIZE(m_szSelectedFile)))
                {
                    hr = S_OK;
                }
            }

            // [-or-]

            // Enumerates the selected files and folders.
            //if (nFiles > 0)
            //{
            //    std::list<std::wstring> selectedFiles;
            //    wchar_t szFileName[MAX_PATH];
            //    for (UINT i = 0; i < nFiles; i++)
            //    {
            //        // Get the next file name.
            //        if (0 != DragQueryFile(hDrop, i, szFileName, ARRAYSIZE(szFileName)))
            //        {
            //            // Add the file name to the list.
            //            selectedFiles.push_back(szFileName);
            //        }
            //    }

            //    // If we found any files we can work with, return S_OK.
            //    if (selectedFiles.size() > 0) 
            //    {
            //        hr = S_OK;
            //    }
            //}

            GlobalUnlock(stm.hGlobal);
        }

        ReleaseStgMedium(&stm);
    }

    // If any value other than S_OK is returned from the method, the context 
    // menu item is not displayed.
    return hr;*/
}

IFACEMETHODIMP VkloudMenuExtention::QueryContextMenu(HMENU menu, UINT indexMenu, UINT idCmdFirst, UINT idCmdLast, UINT flags)
{
    if (CMF_DEFAULTONLY & flags)
    {
        return MAKE_HRESULT(SEVERITY_SUCCESS, 0, USHORT(0));
    }

    // Use either InsertMenu or InsertMenuItem to add menu items.
    // Learn how to add sub-menu from:
    // http://www.codeproject.com/KB/shell/ctxextsubmenu.aspx

    MENUITEMINFO mii = { sizeof(mii) };
    mii.fMask = MIIM_STRING | MIIM_FTYPE | MIIM_ID | MIIM_STATE;
    mii.wID = idCmdFirst + IDM_DISPLAY;
    mii.fType = MFT_STRING;
    mii.dwTypeData = const_cast<PWSTR>(menuText);
    mii.fState = MFS_ENABLED;
    //mii.hbmpItem = static_cast<HBITMAP>(m_hMenuBmp);
    if (!InsertMenuItem(menu, indexMenu, TRUE, &mii))
    {
        return HRESULT_FROM_WIN32(GetLastError());
    }

    // Add a separator.
    MENUITEMINFO sep = { sizeof(sep) };
    sep.fMask = MIIM_TYPE;
    sep.fType = MFT_SEPARATOR;
    if (!InsertMenuItem(menu, indexMenu + 1, TRUE, &sep))
    {
        return HRESULT_FROM_WIN32(GetLastError());
    }

    // Return an HRESULT value with the severity set to SEVERITY_SUCCESS. 
    // Set the code value to the offset of the largest command identifier 
    // that was assigned, plus one (1).
    return MAKE_HRESULT(SEVERITY_SUCCESS, 0, USHORT(IDM_DISPLAY + 1));
}

IFACEMETHODIMP VkloudMenuExtention::InvokeCommand(LPCMINVOKECOMMANDINFO info)
{
    return S_OK;
}

IFACEMETHODIMP VkloudMenuExtention::GetCommandString(UINT_PTR isCommand, UINT flags, UINT* reserved, LPSTR name, UINT cchMax)
{
    return S_OK;
}
