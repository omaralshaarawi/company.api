import { DOCUMENT } from '@angular/common';
import { Injectable, inject } from '@angular/core';
import { clearTranslations, loadTranslations } from '@angular/localize';

declare const __APP_DEFAULT_LANGUAGE__: string;

type Language = 'en' | 'ar';

const LANGUAGE_STORAGE_KEY = 'company-web-language';

@Injectable({ providedIn: 'root' })
export class LanguageService {
  private readonly document = inject(DOCUMENT);
  private language: Language = this.document.documentElement.lang === 'ar' ? 'ar' : 'en';

  get isArabic(): boolean {
    return this.language === 'ar';
  }

  async initialize(): Promise<void> {
    const view = this.document.defaultView;
    const savedLanguage = view?.localStorage.getItem(LANGUAGE_STORAGE_KEY);
    const pathLanguage = view?.location.pathname === '/ar' || view?.location.pathname.startsWith('/ar/')
      ? 'ar'
      : null;
    const defaultLanguage = typeof __APP_DEFAULT_LANGUAGE__ === 'undefined'
      ? 'en'
      : __APP_DEFAULT_LANGUAGE__;

    this.language = savedLanguage === 'ar' || savedLanguage === 'en'
      ? savedLanguage
      : pathLanguage ?? (defaultLanguage === 'ar' ? 'ar' : 'en');

    if (view && this.language === 'ar') {
      const response = await fetch(new URL('locale/messages.ar.xlf', this.document.baseURI));
      if (!response.ok) {
        throw new Error(`Unable to load Arabic translations: ${response.status} ${response.statusText}`);
      }

      const catalog = new DOMParser().parseFromString(await response.text(), 'application/xml');
      const parserError = catalog.querySelector('parsererror');
      if (parserError) {
        throw new Error(`Unable to parse Arabic translations: ${parserError.textContent ?? 'Invalid XLIFF catalog.'}`);
      }

      const translations: Record<string, string> = {};
      for (const unit of Array.from(catalog.getElementsByTagName('trans-unit'))) {
        const id = unit.getAttribute('id');
        const target = unit.getElementsByTagName('target').item(0);
        if (!id || !target) {
          continue;
        }

        translations[id] = Array.from(target.childNodes)
          .map(node => {
            if (node.nodeType === Node.ELEMENT_NODE && (node as Element).localName === 'x') {
              const placeholder = (node as Element).getAttribute('id');
              if (!placeholder) {
                throw new Error(`Arabic translation ${id} contains a placeholder without an ID.`);
              }
              return `{$${placeholder}}`;
            }
            return node.textContent ?? '';
          })
          .join('');
      }

      loadTranslations(translations);
    } else {
      clearTranslations();
    }

    this.document.documentElement.lang = this.language === 'ar' ? 'ar' : 'en';
    this.document.documentElement.dir = this.language === 'ar' ? 'rtl' : 'ltr';

    if (view && pathLanguage) {
      const path = view.location.pathname.replace(/^\/ar(?=\/|$)/, '') || '/';
      view.history.replaceState(view.history.state, '', `${path}${view.location.search}${view.location.hash}`);
    }
  }

  toggleLanguage(): void {
    this.setLanguage(this.isArabic ? 'en' : 'ar');
  }

  setLanguage(language: Language): void {
    if (language !== 'en' && language !== 'ar') {
      throw new Error(`Unsupported language: ${language}`);
    }

    const view = this.document.defaultView;
    if (!view) {
      throw new Error('Cannot change language without a browser window.');
    }

    view.localStorage.setItem(LANGUAGE_STORAGE_KEY, language);
    const path = view.location.pathname.replace(/^\/ar(?=\/|$)/, '') || '/';
    view.location.assign(`${path}${view.location.search}${view.location.hash}`);
  }
}
