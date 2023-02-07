using KarteReport.Interface;
using System.Text;


namespace KarteReport.Utility
{
    public class TemplateGenerator
    {
        private readonly IReportServices _reportService;

        public TemplateGenerator(IReportServices reportService)
        {
            _reportService = reportService;
        }

        public string GetHTMLString(int hpId, long ptId, int sinDate, int hokenPid, bool tenkiByomei, bool syuByomei)
        {
            var rootPath = Environment.CurrentDirectory;

            var karte1Data = _reportService.GetKarte1ReportingData(hpId, ptId, sinDate, hokenPid, tenkiByomei, syuByomei);

            var sb = new StringBuilder();

            sb.Append(@"<!DOCTYPE html>
<html lang=""en"">

<head>
    <meta charset=""UTF-8"">
    <meta http-equiv=""X-UA-Compatible"" content=""IE=edge"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Document</title>
</head>

<body>
    <style>
        /* Copyright 2014 Mozilla Foundation
 *
 * Licensed under the Apache License, Version 2.0 (the ""License"");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an ""AS IS"" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

.textLayer {
  position: absolute;
  text-align: initial;
  left: 0;
  top: 0;
  right: 0;
  bottom: 0;
  overflow: hidden;
  opacity: 1;
  line-height: 1;
  text-size-adjust: none;
  forced-color-adjust: none;
  transform-origin: 0 0;
}

.textLayer span,
.textLayer br {
  color: black;
  position: absolute;
  white-space: pre;
  cursor: text;
  transform-origin: 0% 0%;
}

/* Only necessary in Google Chrome, see issue 14205, and most unfortunately
 * the problem doesn't show up in ""text"" reference tests. */
.textLayer span.markedContent {
  top: 0;
  height: 0;
}

.textLayer .highlight {
  margin: -1px;
  padding: 1px;
  background-color: rgba(180, 0, 170, 1);
  border-radius: 4px;
}

.textLayer .highlight.appended {
  position: initial;
}

.textLayer .highlight.begin {
  border-radius: 4px 0 0 4px;
}

.textLayer .highlight.end {
  border-radius: 0 4px 4px 0;
}

.textLayer .highlight.middle {
  border-radius: 0;
}

.textLayer .highlight.selected {
  background-color: rgba(0, 100, 0, 1);
}

.textLayer ::selection {
  background: AccentColor;
}

/* Avoids https://github.com/mozilla/pdf.js/issues/13840 in Chrome */
.textLayer br::selection {
  background: transparent;
}

.textLayer .endOfContent {
  display: block;
  position: absolute;
  left: 0;
  top: 100%;
  right: 0;
  bottom: 0;
  z-index: -1;
  cursor: default;
  user-select: none;
}

.textLayer .endOfContent.active {
  top: 0;
}


:root {
  --annotation-unfocused-field-background: url(""data:image/svg+xml;charset=UTF-8,<svg width='1px' height='1px' xmlns='http://www.w3.org/2000/svg'><rect width='100%' height='100%' style='fill:rgba(0, 54, 255, 0.13);'/></svg>"");
  --input-focus-border-color: Highlight;
  --input-focus-outline: 1px solid Canvas;
  --input-unfocused-border-color: transparent;
  --input-disabled-border-color: transparent;
  --input-hover-border-color: black;
}

@media (forced-colors: active) {
  :root {
    --input-focus-border-color: CanvasText;
    --input-unfocused-border-color: ActiveText;
    --input-disabled-border-color: GrayText;
    --input-hover-border-color: Highlight;
  }
  .annotationLayer .textWidgetAnnotation input:required,
  .annotationLayer .textWidgetAnnotation textarea:required,
  .annotationLayer .choiceWidgetAnnotation select:required,
  .annotationLayer .buttonWidgetAnnotation.checkBox input:required,
  .annotationLayer .buttonWidgetAnnotation.radioButton input:required {
    outline: 1.5px solid selectedItem;
  }
}

.annotationLayer {
  position: absolute;
  top: 0;
  left: 0;
  pointer-events: none;
  transform-origin: 0 0;
}

.annotationLayer section {
  position: absolute;
  text-align: initial;
  pointer-events: auto;
  box-sizing: border-box;
  transform-origin: 0 0;
}

.annotationLayer .linkAnnotation > a,
.annotationLayer .buttonWidgetAnnotation.pushButton > a {
  position: absolute;
  font-size: 1em;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
}

.annotationLayer .buttonWidgetAnnotation.pushButton > canvas {
  width: 100%;
  height: 100%;
}

.annotationLayer .linkAnnotation > a:hover,
.annotationLayer .buttonWidgetAnnotation.pushButton > a:hover {
  opacity: 0.2;
  background: rgba(255, 255, 0, 1);
  box-shadow: 0 2px 10px rgba(255, 255, 0, 1);
}

.annotationLayer .textAnnotation img {
  position: absolute;
  cursor: pointer;
  width: 100%;
  height: 100%;
}

.annotationLayer .textWidgetAnnotation input,
.annotationLayer .textWidgetAnnotation textarea,
.annotationLayer .choiceWidgetAnnotation select,
.annotationLayer .buttonWidgetAnnotation.checkBox input,
.annotationLayer .buttonWidgetAnnotation.radioButton input {
  background-image: var(--annotation-unfocused-field-background);
  border: 2px solid var(--input-unfocused-border-color);
  box-sizing: border-box;
  font: calc(9px * var(--scale-factor)) sans-serif;
  height: 100%;
  margin: 0;
  vertical-align: top;
  width: 100%;
}

.annotationLayer .textWidgetAnnotation input:required,
.annotationLayer .textWidgetAnnotation textarea:required,
.annotationLayer .choiceWidgetAnnotation select:required,
.annotationLayer .buttonWidgetAnnotation.checkBox input:required,
.annotationLayer .buttonWidgetAnnotation.radioButton input:required {
  outline: 1.5px solid red;
}

.annotationLayer .choiceWidgetAnnotation select option {
  padding: 0;
}

.annotationLayer .buttonWidgetAnnotation.radioButton input {
  border-radius: 50%;
}

.annotationLayer .textWidgetAnnotation textarea {
  resize: none;
}

.annotationLayer .textWidgetAnnotation input[disabled],
.annotationLayer .textWidgetAnnotation textarea[disabled],
.annotationLayer .choiceWidgetAnnotation select[disabled],
.annotationLayer .buttonWidgetAnnotation.checkBox input[disabled],
.annotationLayer .buttonWidgetAnnotation.radioButton input[disabled] {
  background: none;
  border: 2px solid var(--input-disabled-border-color);
  cursor: not-allowed;
}

.annotationLayer .textWidgetAnnotation input:hover,
.annotationLayer .textWidgetAnnotation textarea:hover,
.annotationLayer .choiceWidgetAnnotation select:hover,
.annotationLayer .buttonWidgetAnnotation.checkBox input:hover,
.annotationLayer .buttonWidgetAnnotation.radioButton input:hover {
  border: 2px solid var(--input-hover-border-color);
}
.annotationLayer .textWidgetAnnotation input:hover,
.annotationLayer .textWidgetAnnotation textarea:hover,
.annotationLayer .choiceWidgetAnnotation select:hover,
.annotationLayer .buttonWidgetAnnotation.checkBox input:hover {
  border-radius: 2px;
}

.annotationLayer .textWidgetAnnotation input:focus,
.annotationLayer .textWidgetAnnotation textarea:focus,
.annotationLayer .choiceWidgetAnnotation select:focus {
  background: none;
  border: 2px solid var(--input-focus-border-color);
  border-radius: 2px;
  outline: var(--input-focus-outline);
}

.annotationLayer .buttonWidgetAnnotation.checkBox :focus,
.annotationLayer .buttonWidgetAnnotation.radioButton :focus {
  background-image: none;
  background-color: transparent;
}

.annotationLayer .buttonWidgetAnnotation.checkBox :focus {
  border: 2px solid var(--input-focus-border-color);
  border-radius: 2px;
  outline: var(--input-focus-outline);
}

.annotationLayer .buttonWidgetAnnotation.radioButton :focus {
  border: 2px solid var(--input-focus-border-color);
  outline: var(--input-focus-outline);
}

.annotationLayer .buttonWidgetAnnotation.checkBox input:checked:before,
.annotationLayer .buttonWidgetAnnotation.checkBox input:checked:after,
.annotationLayer .buttonWidgetAnnotation.radioButton input:checked:before {
  background-color: CanvasText;
  content: "";
  display: block;
  position: absolute;
}

.annotationLayer .buttonWidgetAnnotation.checkBox input:checked:before,
.annotationLayer .buttonWidgetAnnotation.checkBox input:checked:after {
  height: 80%;
  left: 45%;
  width: 1px;
}

.annotationLayer .buttonWidgetAnnotation.checkBox input:checked:before {
  transform: rotate(45deg);
}

.annotationLayer .buttonWidgetAnnotation.checkBox input:checked:after {
  transform: rotate(-45deg);
}

.annotationLayer .buttonWidgetAnnotation.radioButton input:checked:before {
  border-radius: 50%;
  height: 50%;
  left: 30%;
  top: 20%;
  width: 50%;
}

.annotationLayer .textWidgetAnnotation input.comb {
  font-family: monospace;
  padding-left: 2px;
  padding-right: 0;
}

.annotationLayer .textWidgetAnnotation input.comb:focus {
  /*
   * Letter spacing is placed on the right side of each character. Hence, the
   * letter spacing of the last character may be placed outside the visible
   * area, causing horizontal scrolling. We avoid this by extending the width
   * when the element has focus and revert this when it loses focus.
   */
  width: 103%;
}

.annotationLayer .buttonWidgetAnnotation.checkBox input,
.annotationLayer .buttonWidgetAnnotation.radioButton input {
  appearance: none;
}

.annotationLayer .popupTriggerArea {
  height: 100%;
  width: 100%;
}

.annotationLayer .popupWrapper {
  position: absolute;
  font-size: calc(9px * var(--scale-factor));
  width: 100%;
  min-width: calc(180px * var(--scale-factor));
  pointer-events: none;
}

.annotationLayer .popup {
  position: absolute;
  max-width: calc(180px * var(--scale-factor));
  background-color: rgba(255, 255, 153, 1);
  box-shadow: 0 calc(2px * var(--scale-factor)) calc(5px * var(--scale-factor))
    rgba(136, 136, 136, 1);
  border-radius: calc(2px * var(--scale-factor));
  padding: calc(6px * var(--scale-factor));
  margin-left: calc(5px * var(--scale-factor));
  cursor: pointer;
  font: message-box;
  white-space: normal;
  word-wrap: break-word;
  pointer-events: auto;
}

.annotationLayer .popup > * {
  font-size: calc(9px * var(--scale-factor));
}

.annotationLayer .popup h1 {
  display: inline-block;
}

.annotationLayer .popupDate {
  display: inline-block;
  margin-left: calc(5px * var(--scale-factor));
}

.annotationLayer .popupContent {
  border-top: 1px solid rgba(51, 51, 51, 1);
  margin-top: calc(2px * var(--scale-factor));
  padding-top: calc(2px * var(--scale-factor));
}

.annotationLayer .richText > * {
  white-space: pre-wrap;
  font-size: calc(9px * var(--scale-factor));
}

.annotationLayer .highlightAnnotation,
.annotationLayer .underlineAnnotation,
.annotationLayer .squigglyAnnotation,
.annotationLayer .strikeoutAnnotation,
.annotationLayer .freeTextAnnotation,
.annotationLayer .lineAnnotation svg line,
.annotationLayer .squareAnnotation svg rect,
.annotationLayer .circleAnnotation svg ellipse,
.annotationLayer .polylineAnnotation svg polyline,
.annotationLayer .polygonAnnotation svg polygon,
.annotationLayer .caretAnnotation,
.annotationLayer .inkAnnotation svg polyline,
.annotationLayer .stampAnnotation,
.annotationLayer .fileAttachmentAnnotation {
  cursor: pointer;
}

.annotationLayer section svg {
  position: absolute;
  width: 100%;
  height: 100%;
}

.annotationLayer .annotationTextContent {
  position: absolute;
  width: 100%;
  height: 100%;
  opacity: 0;
  color: transparent;
  user-select: none;
  pointer-events: none;
}

.annotationLayer .annotationTextContent span {
  width: 100%;
  display: inline-block;
}


:root {
  --xfa-unfocused-field-background: url(""data:image/svg+xml;charset=UTF-8,<svg width='1px' height='1px' xmlns='http://www.w3.org/2000/svg'><rect width='100%' height='100%' style='fill:rgba(0, 54, 255, 0.13);'/></svg>"");
  --xfa-focus-outline: auto;
}

@media (forced-colors: active) {
  :root {
    --xfa-focus-outline: 2px solid CanvasText;
  }
  .xfaLayer *:required {
    outline: 1.5px solid selectedItem;
  }
}

.xfaLayer {
  background-color: transparent;
}

.xfaLayer .highlight {
  margin: -1px;
  padding: 1px;
  background-color: rgba(239, 203, 237, 1);
  border-radius: 4px;
}

.xfaLayer .highlight.appended {
  position: initial;
}

.xfaLayer .highlight.begin {
  border-radius: 4px 0 0 4px;
}

.xfaLayer .highlight.end {
  border-radius: 0 4px 4px 0;
}

.xfaLayer .highlight.middle {
  border-radius: 0;
}

.xfaLayer .highlight.selected {
  background-color: rgba(203, 223, 203, 1);
}

.xfaPage {
  overflow: hidden;
  position: relative;
}

.xfaContentarea {
  position: absolute;
}

.xfaPrintOnly {
  display: none;
}

.xfaLayer {
  position: absolute;
  text-align: initial;
  top: 0;
  left: 0;
  transform-origin: 0 0;
  line-height: 1.2;
}

.xfaLayer * {
  color: inherit;
  font: inherit;
  font-style: inherit;
  font-weight: inherit;
  font-kerning: inherit;
  letter-spacing: -0.01px;
  text-align: inherit;
  text-decoration: inherit;
  box-sizing: border-box;
  background-color: transparent;
  padding: 0;
  margin: 0;
  pointer-events: auto;
  line-height: inherit;
}

.xfaLayer *:required {
  outline: 1.5px solid red;
}

.xfaLayer div {
  pointer-events: none;
}

.xfaLayer svg {
  pointer-events: none;
}

.xfaLayer svg * {
  pointer-events: none;
}

.xfaLayer a {
  color: blue;
}

.xfaRich li {
  margin-left: 3em;
}

.xfaFont {
  color: black;
  font-weight: normal;
  font-kerning: none;
  font-size: 10px;
  font-style: normal;
  letter-spacing: 0;
  text-decoration: none;
  vertical-align: 0;
}

.xfaCaption {
  overflow: hidden;
  flex: 0 0 auto;
}

.xfaCaptionForCheckButton {
  overflow: hidden;
  flex: 1 1 auto;
}

.xfaLabel {
  height: 100%;
  width: 100%;
}

.xfaLeft {
  display: flex;
  flex-direction: row;
  align-items: center;
}

.xfaRight {
  display: flex;
  flex-direction: row-reverse;
  align-items: center;
}

.xfaLeft > .xfaCaption,
.xfaLeft > .xfaCaptionForCheckButton,
.xfaRight > .xfaCaption,
.xfaRight > .xfaCaptionForCheckButton {
  max-height: 100%;
}

.xfaTop {
  display: flex;
  flex-direction: column;
  align-items: flex-start;
}

.xfaBottom {
  display: flex;
  flex-direction: column-reverse;
  align-items: flex-start;
}

.xfaTop > .xfaCaption,
.xfaTop > .xfaCaptionForCheckButton,
.xfaBottom > .xfaCaption,
.xfaBottom > .xfaCaptionForCheckButton {
  width: 100%;
}

.xfaBorder {
  background-color: transparent;
  position: absolute;
  pointer-events: none;
}

.xfaWrapped {
  width: 100%;
  height: 100%;
}

.xfaTextfield:focus,
.xfaSelect:focus {
  background-image: none;
  background-color: transparent;
  outline: var(--xfa-focus-outline);
  outline-offset: -1px;
}

.xfaCheckbox:focus,
.xfaRadio:focus {
  outline: var(--xfa-focus-outline);
}

.xfaTextfield,
.xfaSelect {
  height: 100%;
  width: 100%;
  flex: 1 1 auto;
  border: none;
  resize: none;
  background-image: var(--xfa-unfocused-field-background);
}

.xfaSelect {
  padding-inline: 2px;
}

.xfaTop > .xfaTextfield,
.xfaTop > .xfaSelect,
.xfaBottom > .xfaTextfield,
.xfaBottom > .xfaSelect {
  flex: 0 1 auto;
}

.xfaButton {
  cursor: pointer;
  width: 100%;
  height: 100%;
  border: none;
  text-align: center;
}

.xfaLink {
  width: 100%;
  height: 100%;
  position: absolute;
  top: 0;
  left: 0;
}

.xfaCheckbox,
.xfaRadio {
  width: 100%;
  height: 100%;
  flex: 0 0 auto;
  border: none;
}

.xfaRich {
  white-space: pre-wrap;
  width: 100%;
  height: 100%;
}

.xfaImage {
  object-position: left top;
  object-fit: contain;
  width: 100%;
  height: 100%;
}

.xfaLrTb,
.xfaRlTb,
.xfaTb {
  display: flex;
  flex-direction: column;
  align-items: stretch;
}

.xfaLr {
  display: flex;
  flex-direction: row;
  align-items: stretch;
}

.xfaRl {
  display: flex;
  flex-direction: row-reverse;
  align-items: stretch;
}

.xfaTb > div {
  justify-content: left;
}

.xfaPosition {
  position: relative;
}

.xfaArea {
  position: relative;
}

.xfaValignMiddle {
  display: flex;
  align-items: center;
}

.xfaTable {
  display: flex;
  flex-direction: column;
  align-items: stretch;
}

.xfaTable .xfaRow {
  display: flex;
  flex-direction: row;
  align-items: stretch;
}

.xfaTable .xfaRlRow {
  display: flex;
  flex-direction: row-reverse;
  align-items: stretch;
  flex: 1;
}

.xfaTable .xfaRlRow > div {
  flex: 1;
}

.xfaNonInteractive input,
.xfaNonInteractive textarea,
.xfaDisabled input,
.xfaDisabled textarea,
.xfaReadOnly input,
.xfaReadOnly textarea {
  background: initial;
}

@media print {
  .xfaTextfield,
  .xfaSelect {
    background: transparent;
  }

  .xfaSelect {
    appearance: none;
    text-indent: 1px;
    text-overflow: "";
  }
}


:root {
  --focus-outline: solid 2px blue;
  --hover-outline: dashed 2px blue;
  --freetext-line-height: 1.35;
  --freetext-padding: 2px;
  --editorFreeText-editing-cursor: text;
  --editorInk-editing-cursor: url(images/cursor-editorInk.svg) 0 16, pointer;
}

@media (min-resolution: 1.1dppx) {
  :root {
    --editorFreeText-editing-cursor: url(images/cursor-editorFreeText.svg) 0 16,
      text;
  }
}

@media (forced-colors: active) {
  :root {
    --focus-outline: solid 3px ButtonText;
    --hover-outline: dashed 3px ButtonText;
  }
}

[data-editor-rotation=""90""] {
  transform: rotate(90deg);
}
[data-editor-rotation=""180""] {
  transform: rotate(180deg);
}
[data-editor-rotation=""270""] {
  transform: rotate(270deg);
}

.annotationEditorLayer {
  background: transparent;
  position: absolute;
  top: 0;
  left: 0;
  font-size: calc(100px * var(--scale-factor));
  transform-origin: 0 0;
  cursor: auto;
  z-index: 20000;
}

.annotationEditorLayer.freeTextEditing {
  cursor: var(--editorFreeText-editing-cursor);
}

.annotationEditorLayer.inkEditing {
  cursor: var(--editorInk-editing-cursor);
}

.annotationEditorLayer .selectedEditor {
  outline: var(--focus-outline);
  resize: none;
}

.annotationEditorLayer .freeTextEditor {
  position: absolute;
  background: transparent;
  border-radius: 3px;
  padding: calc(var(--freetext-padding) * var(--scale-factor));
  resize: none;
  width: auto;
  height: auto;
  z-index: 1;
  transform-origin: 0 0;
  touch-action: none;
  cursor: auto;
}

.annotationEditorLayer .freeTextEditor .internal {
  background: transparent;
  border: none;
  top: 0;
  left: 0;
  overflow: visible;
  white-space: nowrap;
  resize: none;
  font: 10px sans-serif;
  line-height: var(--freetext-line-height);
}

.annotationEditorLayer .freeTextEditor .overlay {
  position: absolute;
  display: none;
  background: transparent;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
}

.annotationEditorLayer .freeTextEditor .overlay.enabled {
  display: block;
}

.annotationEditorLayer .freeTextEditor .internal:empty::before {
  content: attr(default-content);
  color: gray;
}

.annotationEditorLayer .freeTextEditor .internal:focus {
  outline: none;
}

.annotationEditorLayer .inkEditor.disabled {
  resize: none;
}

.annotationEditorLayer .inkEditor.disabled.selectedEditor {
  resize: horizontal;
}

.annotationEditorLayer .freeTextEditor:hover:not(.selectedEditor),
.annotationEditorLayer .inkEditor:hover:not(.selectedEditor) {
  outline: var(--hover-outline);
}

.annotationEditorLayer .inkEditor {
  position: absolute;
  background: transparent;
  border-radius: 3px;
  overflow: auto;
  width: 100%;
  height: 100%;
  z-index: 1;
  transform-origin: 0 0;
  cursor: auto;
}

.annotationEditorLayer .inkEditor.editing {
  resize: none;
  cursor: inherit;
}

.annotationEditorLayer .inkEditor .inkEditorCanvas {
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  touch-action: none;
}

:root {
  --viewer-container-height: 0;
  --pdfViewer-padding-bottom: 0;
  --page-margin: 1px auto -8px;
  --page-border: 9px solid transparent;
  --page-border-image: url(images/shadow.png) 9 9 repeat;
  --spreadHorizontalWrapped-margin-LR: -3.5px;
  --scale-factor: 1;
}

@media screen and (forced-colors: active) {
  :root {
    --pdfViewer-padding-bottom: 9px;
    --page-margin: 8px auto -1px;
    --page-border: 1px solid CanvasText;
    --page-border-image: none;
    --spreadHorizontalWrapped-margin-LR: 3.5px;
  }
}

[data-main-rotation=""90""] {
  transform: rotate(90deg) translateY(-100%);
}
[data-main-rotation=""180""] {
  transform: rotate(180deg) translate(-100%, -100%);
}
[data-main-rotation=""270""] {
  transform: rotate(270deg) translateX(-100%);
}

.pdfViewer {
  padding-bottom: var(--pdfViewer-padding-bottom);
}

.pdfViewer .canvasWrapper {
  overflow: hidden;
}

.pdfViewer .page {
  direction: ltr;
  width: 816px;
  height: 1056px;
  margin: var(--page-margin);
  position: relative;
  overflow: visible;
  border: var(--page-border);
  border-image: var(--page-border-image);
  background-clip: content-box;
  background-color: rgba(255, 255, 255, 1);
}

.pdfViewer .dummyPage {
  position: relative;
  width: 0;
  height: var(--viewer-container-height);
}

.pdfViewer.removePageBorders .page {
  margin: 0 auto 10px;
  border: none;
}

.pdfViewer.singlePageView {
  display: inline-block;
}

.pdfViewer.singlePageView .page {
  margin: 0;
  border: none;
}

.pdfViewer.scrollHorizontal,
.pdfViewer.scrollWrapped,
.spread {
  margin-left: 3.5px;
  margin-right: 3.5px;
  text-align: center;
}

.pdfViewer.scrollHorizontal,
.spread {
  white-space: nowrap;
}

.pdfViewer.removePageBorders,
.pdfViewer.scrollHorizontal .spread,
.pdfViewer.scrollWrapped .spread {
  margin-left: 0;
  margin-right: 0;
}

.spread .page,
.spread .dummyPage,
.pdfViewer.scrollHorizontal .page,
.pdfViewer.scrollWrapped .page,
.pdfViewer.scrollHorizontal .spread,
.pdfViewer.scrollWrapped .spread {
  display: inline-block;
  vertical-align: middle;
}

.spread .page,
.pdfViewer.scrollHorizontal .page,
.pdfViewer.scrollWrapped .page {
  margin-left: var(--spreadHorizontalWrapped-margin-LR);
  margin-right: var(--spreadHorizontalWrapped-margin-LR);
}

.pdfViewer.removePageBorders .spread .page,
.pdfViewer.removePageBorders.scrollHorizontal .page,
.pdfViewer.removePageBorders.scrollWrapped .page {
  margin-left: 5px;
  margin-right: 5px;
}

.pdfViewer .page canvas {
  margin: 0;
  display: block;
}

.pdfViewer .page canvas[hidden] {
  display: none;
}

.pdfViewer .page .loadingIcon {
  position: absolute;
  display: block;
  left: 0;
  top: 0;
  right: 0;
  bottom: 0;
  background: url(""images/loading-icon.gif"") center no-repeat;
}
.pdfViewer .page .loadingIcon.notVisible {
  background: none;
}

.pdfViewer.enablePermissions .textLayer span {
  user-select: none !important;
  cursor: not-allowed;
}

.pdfPresentationMode .pdfViewer {
  padding-bottom: 0;
}

.pdfPresentationMode .spread {
  margin: 0;
}

.pdfPresentationMode .pdfViewer .page {
  margin: 0 auto;
  border: 2px solid transparent;
}

:root {
  --dir-factor: 1;
  --sidebar-width: 200px;
  --sidebar-transition-duration: 200ms;
  --sidebar-transition-timing-function: ease;
  --scale-select-width: 140px;

  --toolbar-icon-opacity: 0.7;
  --doorhanger-icon-opacity: 0.9;

  --main-color: rgba(12, 12, 13, 1);
  --body-bg-color: rgba(237, 237, 240, 1);
  --progressBar-percent: 0%;
  --progressBar-end-offset: 0;
  --progressBar-color: rgba(10, 132, 255, 1);
  --progressBar-indeterminate-bg-color: rgba(221, 221, 222, 1);
  --progressBar-indeterminate-blend-color: rgba(116, 177, 239, 1);
  --scrollbar-color: auto;
  --scrollbar-bg-color: auto;
  --toolbar-icon-bg-color: rgba(0, 0, 0, 1);
  --toolbar-icon-hover-bg-color: rgba(0, 0, 0, 1);

  --sidebar-narrow-bg-color: rgba(237, 237, 240, 0.9);
  --sidebar-toolbar-bg-color: rgba(245, 246, 247, 1);
  --toolbar-bg-color: rgba(249, 249, 250, 1);
  --toolbar-border-color: rgba(204, 204, 204, 1);
  --toolbar-box-shadow: 0 1px 0 var(--toolbar-border-color);
  --toolbar-border-bottom: none;
  --toolbarSidebar-box-shadow: inset calc(-1px * var(--dir-factor)) 0 0
      rgba(0, 0, 0, 0.25),
    0 1px 0 rgba(0, 0, 0, 0.15), 0 0 1px rgba(0, 0, 0, 0.1);
  --toolbarSidebar-border-bottom: none;
  --button-hover-color: rgba(221, 222, 223, 1);
  --toggled-btn-color: rgba(0, 0, 0, 1);
  --toggled-btn-bg-color: rgba(0, 0, 0, 0.3);
  --toggled-hover-active-btn-color: rgba(0, 0, 0, 0.4);
  --dropdown-btn-bg-color: rgba(215, 215, 219, 1);
  --dropdown-btn-border: none;
  --separator-color: rgba(0, 0, 0, 0.3);
  --field-color: rgba(6, 6, 6, 1);
  --field-bg-color: rgba(255, 255, 255, 1);
  --field-border-color: rgba(187, 187, 188, 1);
  --treeitem-color: rgba(0, 0, 0, 0.8);
  --treeitem-hover-color: rgba(0, 0, 0, 0.9);
  --treeitem-selected-color: rgba(0, 0, 0, 0.9);
  --treeitem-selected-bg-color: rgba(0, 0, 0, 0.25);
  --sidebaritem-bg-color: rgba(0, 0, 0, 0.15);
  --doorhanger-bg-color: rgba(255, 255, 255, 1);
  --doorhanger-border-color: rgba(12, 12, 13, 0.2);
  --doorhanger-hover-color: rgba(12, 12, 13, 1);
  --doorhanger-hover-bg-color: rgba(237, 237, 237, 1);
  --doorhanger-separator-color: rgba(222, 222, 222, 1);
  --dialog-button-border: none;
  --dialog-button-bg-color: rgba(12, 12, 13, 0.1);
  --dialog-button-hover-bg-color: rgba(12, 12, 13, 0.3);

  --loading-icon: url(images/loading.svg);
  --treeitem-expanded-icon: url(images/treeitem-expanded.svg);
  --treeitem-collapsed-icon: url(images/treeitem-collapsed.svg);
  --toolbarButton-editorFreeText-icon: url(images/toolbarButton-editorFreeText.svg);
  --toolbarButton-editorInk-icon: url(images/toolbarButton-editorInk.svg);
  --toolbarButton-menuArrow-icon: url(images/toolbarButton-menuArrow.svg);
  --toolbarButton-sidebarToggle-icon: url(images/toolbarButton-sidebarToggle.svg);
  --toolbarButton-secondaryToolbarToggle-icon: url(images/toolbarButton-secondaryToolbarToggle.svg);
  --toolbarButton-pageUp-icon: url(images/toolbarButton-pageUp.svg);
  --toolbarButton-pageDown-icon: url(images/toolbarButton-pageDown.svg);
  --toolbarButton-zoomOut-icon: url(images/toolbarButton-zoomOut.svg);
  --toolbarButton-zoomIn-icon: url(images/toolbarButton-zoomIn.svg);
  --toolbarButton-presentationMode-icon: url(images/toolbarButton-presentationMode.svg);
  --toolbarButton-print-icon: url(images/toolbarButton-print.svg);
  --toolbarButton-download-icon: url(images/toolbarButton-download.svg);
  --toolbarButton-bookmark-icon: url(images/toolbarButton-bookmark.svg);
  --toolbarButton-viewThumbnail-icon: url(images/toolbarButton-viewThumbnail.svg);
  --toolbarButton-viewOutline-icon: url(images/toolbarButton-viewOutline.svg);
  --toolbarButton-viewAttachments-icon: url(images/toolbarButton-viewAttachments.svg);
  --toolbarButton-viewLayers-icon: url(images/toolbarButton-viewLayers.svg);
  --toolbarButton-currentOutlineItem-icon: url(images/toolbarButton-currentOutlineItem.svg);
  --toolbarButton-search-icon: url(images/toolbarButton-search.svg);
  --findbarButton-previous-icon: url(images/findbarButton-previous.svg);
  --findbarButton-next-icon: url(images/findbarButton-next.svg);
  --secondaryToolbarButton-firstPage-icon: url(images/secondaryToolbarButton-firstPage.svg);
  --secondaryToolbarButton-lastPage-icon: url(images/secondaryToolbarButton-lastPage.svg);
  --secondaryToolbarButton-rotateCcw-icon: url(images/secondaryToolbarButton-rotateCcw.svg);
  --secondaryToolbarButton-rotateCw-icon: url(images/secondaryToolbarButton-rotateCw.svg);
  --secondaryToolbarButton-selectTool-icon: url(images/secondaryToolbarButton-selectTool.svg);
  --secondaryToolbarButton-handTool-icon: url(images/secondaryToolbarButton-handTool.svg);
  --secondaryToolbarButton-scrollPage-icon: url(images/secondaryToolbarButton-scrollPage.svg);
  --secondaryToolbarButton-scrollVertical-icon: url(images/secondaryToolbarButton-scrollVertical.svg);
  --secondaryToolbarButton-scrollHorizontal-icon: url(images/secondaryToolbarButton-scrollHorizontal.svg);
  --secondaryToolbarButton-scrollWrapped-icon: url(images/secondaryToolbarButton-scrollWrapped.svg);
  --secondaryToolbarButton-spreadNone-icon: url(images/secondaryToolbarButton-spreadNone.svg);
  --secondaryToolbarButton-spreadOdd-icon: url(images/secondaryToolbarButton-spreadOdd.svg);
  --secondaryToolbarButton-spreadEven-icon: url(images/secondaryToolbarButton-spreadEven.svg);
  --secondaryToolbarButton-documentProperties-icon: url(images/secondaryToolbarButton-documentProperties.svg);
}

:root:dir(rtl) {
  --dir-factor: -1;
}

@media (prefers-color-scheme: dark) {
  :root {
    --main-color: rgba(249, 249, 250, 1);
    --body-bg-color: rgba(42, 42, 46, 1);
    --progressBar-color: rgba(0, 96, 223, 1);
    --progressBar-indeterminate-bg-color: rgba(40, 40, 43, 1);
    --progressBar-indeterminate-blend-color: rgba(20, 68, 133, 1);
    --scrollbar-color: rgba(121, 121, 123, 1);
    --scrollbar-bg-color: rgba(35, 35, 39, 1);
    --toolbar-icon-bg-color: rgba(255, 255, 255, 1);
    --toolbar-icon-hover-bg-color: rgba(255, 255, 255, 1);

    --sidebar-narrow-bg-color: rgba(42, 42, 46, 0.9);
    --sidebar-toolbar-bg-color: rgba(50, 50, 52, 1);
    --toolbar-bg-color: rgba(56, 56, 61, 1);
    --toolbar-border-color: rgba(12, 12, 13, 1);
    --button-hover-color: rgba(102, 102, 103, 1);
    --toggled-btn-color: rgba(255, 255, 255, 1);
    --toggled-btn-bg-color: rgba(0, 0, 0, 0.3);
    --toggled-hover-active-btn-color: rgba(0, 0, 0, 0.4);
    --dropdown-btn-bg-color: rgba(74, 74, 79, 1);
    --separator-color: rgba(0, 0, 0, 0.3);
    --field-color: rgba(250, 250, 250, 1);
    --field-bg-color: rgba(64, 64, 68, 1);
    --field-border-color: rgba(115, 115, 115, 1);
    --treeitem-color: rgba(255, 255, 255, 0.8);
    --treeitem-hover-color: rgba(255, 255, 255, 0.9);
    --treeitem-selected-color: rgba(255, 255, 255, 0.9);
    --treeitem-selected-bg-color: rgba(255, 255, 255, 0.25);
    --sidebaritem-bg-color: rgba(255, 255, 255, 0.15);
    --doorhanger-bg-color: rgba(74, 74, 79, 1);
    --doorhanger-border-color: rgba(39, 39, 43, 1);
    --doorhanger-hover-color: rgba(249, 249, 250, 1);
    --doorhanger-hover-bg-color: rgba(93, 94, 98, 1);
    --doorhanger-separator-color: rgba(92, 92, 97, 1);
    --dialog-button-bg-color: rgba(92, 92, 97, 1);
    --dialog-button-hover-bg-color: rgba(115, 115, 115, 1);

    /* This image is used in <input> elements, which unfortunately means that
     * the `mask-image` approach used with all of the other images doesn't work
     * here; hence why we still have two versions of this particular image. */
    --loading-icon: url(images/loading-dark.svg);
  }
}

@media screen and (forced-colors: active) {
  :root {
    --button-hover-color: Highlight;
    --doorhanger-hover-bg-color: Highlight;
    --toolbar-icon-opacity: 1;
    --toolbar-icon-bg-color: ButtonText;
    --toolbar-icon-hover-bg-color: ButtonFace;
    --toolbar-border-color: CanvasText;
    --toolbar-border-bottom: 1px solid var(--toolbar-border-color);
    --toolbar-box-shadow: none;
    --toggled-btn-color: HighlightText;
    --toggled-btn-bg-color: LinkText;
    --doorhanger-hover-color: ButtonFace;
    --doorhanger-border-color-whcm: 1px solid ButtonText;
    --doorhanger-triangle-opacity-whcm: 0;
    --dialog-button-border: 1px solid Highlight;
    --dialog-button-hover-bg-color: Highlight;
    --dialog-button-hover-color: ButtonFace;
    --dropdown-btn-border: 1px solid ButtonText;
    --field-border-color: ButtonText;
    --main-color: CanvasText;
    --separator-color: GrayText;
    --doorhanger-separator-color: GrayText;
    --toolbarSidebar-box-shadow: none;
    --toolbarSidebar-border-bottom: 1px solid var(--toolbar-border-color);
  }
}

* {
  padding: 0;
  margin: 0;
}

html,
body {
  height: 100%;
  width: 100%;
}

body {
  background-color: var(--body-bg-color);
  scrollbar-color: var(--scrollbar-color) var(--scrollbar-bg-color);
}

.hidden,
[hidden] {
  display: none !important;
}

#viewerContainer.pdfPresentationMode:fullscreen {
  top: 0;
  background-color: rgba(0, 0, 0, 1);
  width: 100%;
  height: 100%;
  overflow: hidden;
  cursor: none;
  user-select: none;
}

.pdfPresentationMode:fullscreen section:not([data-internal-link]) {
  pointer-events: none;
}

.pdfPresentationMode:fullscreen .textLayer span {
  cursor: none;
}

.pdfPresentationMode.pdfPresentationModeControls > *,
.pdfPresentationMode.pdfPresentationModeControls .textLayer span {
  cursor: default;
}

#outerContainer {
  width: 100%;
  height: 100%;
  position: relative;
}

#sidebarContainer {
  position: absolute;
  top: 32px;
  bottom: 0;
  inset-inline-start: calc(-1 * var(--sidebar-width));
  width: var(--sidebar-width);
  visibility: hidden;
  z-index: 100;
  font: message-box;
  border-top: 1px solid rgba(51, 51, 51, 1);
  border-inline-end: var(--doorhanger-border-color-whcm);
  transition-property: inset-inline-start;
  transition-duration: var(--sidebar-transition-duration);
  transition-timing-function: var(--sidebar-transition-timing-function);
}

#outerContainer.sidebarMoving #sidebarContainer,
#outerContainer.sidebarOpen #sidebarContainer {
  visibility: visible;
}
#outerContainer.sidebarOpen #sidebarContainer {
  inset-inline-start: 0;
}

#mainContainer {
  position: absolute;
  top: 0;
  right: 0;
  bottom: 0;
  left: 0;
  min-width: 350px;
}

#sidebarContent {
  top: 32px;
  bottom: 0;
  inset-inline-start: 0;
  overflow: auto;
  position: absolute;
  width: 100%;
  background-color: rgba(0, 0, 0, 0.1);
  box-shadow: inset calc(-1px * var(--dir-factor)) 0 0 rgba(0, 0, 0, 0.25);
}

#viewerContainer {
  overflow: auto;
  position: absolute;
  top: 32px;
  right: 0;
  bottom: 0;
  left: 0;
  outline: none;
}
#viewerContainer:not(.pdfPresentationMode) {
  transition-duration: var(--sidebar-transition-duration);
  transition-timing-function: var(--sidebar-transition-timing-function);
}

#outerContainer.sidebarOpen #viewerContainer:not(.pdfPresentationMode) {
  inset-inline-start: var(--sidebar-width);
  transition-property: inset-inline-start;
}

.toolbar {
  position: relative;
  left: 0;
  right: 0;
  z-index: 9999;
  cursor: default;
  font: message-box;
}

.toolbar input,
.toolbar button,
.toolbar select,
.secondaryToolbar input,
.secondaryToolbar button,
.secondaryToolbar a,
.secondaryToolbar select,
.editorParamsToolbar input,
.editorParamsToolbar button,
.editorParamsToolbar select,
.findbar input,
.findbar button,
.findbar select,
#sidebarContainer input,
#sidebarContainer button,
#sidebarContainer select {
  outline: none;
  font: message-box;
}

#toolbarContainer {
  width: 100%;
}

#toolbarSidebar {
  width: 100%;
  height: 32px;
  background-color: var(--sidebar-toolbar-bg-color);
  box-shadow: var(--toolbarSidebar-box-shadow);
  border-bottom: var(--toolbarSidebar-border-bottom);
}

#sidebarResizer {
  position: absolute;
  top: 0;
  bottom: 0;
  inset-inline-end: -6px;
  width: 6px;
  z-index: 200;
  cursor: ew-resize;
}

#toolbarContainer,
.findbar,
.secondaryToolbar,
.editorParamsToolbar {
  position: relative;
  height: 32px;
  background-color: var(--toolbar-bg-color);
  box-shadow: var(--toolbar-box-shadow);
  border-bottom: var(--toolbar-border-bottom);
}

#toolbarViewer {
  height: 32px;
}

#loadingBar {
  position: absolute;
  inset-inline: 0 var(--progressBar-end-offset);
  height: 4px;
  background-color: var(--body-bg-color);
  border-bottom: 1px solid var(--toolbar-border-color);
  transition-property: inset-inline-start;
  transition-duration: var(--sidebar-transition-duration);
  transition-timing-function: var(--sidebar-transition-timing-function);
}

#outerContainer.sidebarOpen #loadingBar {
  inset-inline-start: var(--sidebar-width);
}

#loadingBar .progress {
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  transform: scaleX(var(--progressBar-percent));
  transform-origin: 0 0;
  height: 100%;
  background-color: var(--progressBar-color);
  overflow: hidden;
  transition: transform 200ms;
}

@keyframes progressIndeterminate {
  0% {
    transform: translateX(-142px);
  }
  100% {
    transform: translateX(0);
  }
}

#loadingBar.indeterminate .progress {
  transform: none;
  background-color: var(--progressBar-indeterminate-bg-color);
  transition: none;
}

#loadingBar.indeterminate .progress .glimmer {
  position: absolute;
  top: 0;
  left: 0;
  height: 100%;
  width: calc(100% + 150px);
  background: repeating-linear-gradient(
    135deg,
    var(--progressBar-indeterminate-blend-color) 0,
    var(--progressBar-indeterminate-bg-color) 5px,
    var(--progressBar-indeterminate-bg-color) 45px,
    var(--progressBar-color) 55px,
    var(--progressBar-color) 95px,
    var(--progressBar-indeterminate-blend-color) 100px
  );
  animation: progressIndeterminate 1s linear infinite;
}

#outerContainer.sidebarResizing #sidebarContainer,
#outerContainer.sidebarResizing #viewerContainer,
#outerContainer.sidebarResizing #loadingBar {
  /* Improve responsiveness and avoid visual glitches when the sidebar is resized. */
  transition-duration: 0s;
}

.findbar,
.secondaryToolbar,
.editorParamsToolbar {
  top: 32px;
  position: absolute;
  z-index: 30000;
  height: auto;
  padding: 0 4px;
  margin: 4px 2px;
  font: message-box;
  font-size: 12px;
  line-height: 14px;
  text-align: left;
  cursor: default;
}

.findbar {
  inset-inline-start: 64px;
  min-width: 300px;
  background-color: var(--toolbar-bg-color);
}
.findbar > div {
  height: 32px;
}
.findbar > div#findbarInputContainer {
  margin-inline-end: 4px;
}
.findbar.wrapContainers > div,
.findbar.wrapContainers > div#findbarMessageContainer > * {
  clear: both;
}
.findbar.wrapContainers > div#findbarMessageContainer {
  height: auto;
}

.findbar input[type=""checkbox""] {
  pointer-events: none;
}

.findbar label {
  user-select: none;
}

.findbar label:hover,
.findbar input:focus-visible + label {
  color: var(--toggled-btn-color);
  background-color: var(--button-hover-color);
}

.findbar .toolbarField[type=""checkbox""]:checked + .toolbarLabel {
  background-color: var(--toggled-btn-bg-color) !important;
  color: var(--toggled-btn-color);
}

#findInput {
  width: 200px;
}
#findInput::placeholder {
  font-style: normal;
}
#findInput[data-status=""pending""] {
  background-image: var(--loading-icon);
  background-repeat: no-repeat;
  background-position: calc(50% + 48% * var(--dir-factor));
}
#findInput[data-status=""notFound""] {
  background-color: rgba(255, 102, 102, 1);
}

.secondaryToolbar,
.editorParamsToolbar {
  padding: 6px 0 10px;
  inset-inline-end: 4px;
  height: auto;
  background-color: var(--doorhanger-bg-color);
}

.editorParamsToolbarContainer {
  width: 220px;
  margin-bottom: -4px;
}

.editorParamsToolbarContainer > .editorParamsSetter {
  min-height: 26px;
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding-inline: 10px;
}

.editorParamsToolbarContainer .editorParamsLabel {
  padding-inline-end: 10px;
  flex: none;
  color: var(--main-color);
}

.editorParamsToolbarContainer .editorParamsColor {
  width: 32px;
  height: 32px;
  flex: none;
}

.editorParamsToolbarContainer .editorParamsSlider {
  background-color: transparent;
  width: 90px;
  flex: 0 1 0;
}

.editorParamsToolbarContainer .editorParamsSlider::-moz-range-progress {
  background-color: black;
}

.editorParamsToolbarContainer .editorParamsSlider::-moz-range-track {
  background-color: black;
}

.editorParamsToolbarContainer .editorParamsSlider::-moz-range-thumb {
  background-color: white;
}

#secondaryToolbarButtonContainer {
  max-width: 220px;
  min-height: 26px;
  max-height: calc(var(--viewer-container-height) - 40px);
  overflow-y: auto;
  margin-bottom: -4px;
}

#editorInkParamsToolbar {
  inset-inline-end: 40px;
  background-color: var(--toolbar-bg-color);
}

#editorFreeTextParamsToolbar {
  inset-inline-end: 68px;
  background-color: var(--toolbar-bg-color);
}

.doorHanger,
.doorHangerRight {
  border-radius: 2px;
  box-shadow: 0 1px 5px var(--doorhanger-border-color),
    0 0 0 1px var(--doorhanger-border-color);
  border: var(--doorhanger-border-color-whcm);
}
.doorHanger:after,
.doorHanger:before,
.doorHangerRight:after,
.doorHangerRight:before {
  bottom: 100%;
  border: 8px solid rgba(0, 0, 0, 0);
  content: "" "";
  height: 0;
  width: 0;
  position: absolute;
  pointer-events: none;
  opacity: var(--doorhanger-triangle-opacity-whcm);
}
.doorHanger:after {
  inset-inline-start: 10px;
  margin-inline-start: -8px;
  border-bottom-color: var(--toolbar-bg-color);
}
.doorHangerRight:after {
  inset-inline-end: 10px;
  margin-inline-end: -8px;
  border-bottom-color: var(--doorhanger-bg-color);
}
.doorHanger:before,
.doorHangerRight:before {
  border-bottom-color: var(--doorhanger-border-color);
  border-width: 9px;
}
.doorHanger:before {
  inset-inline-start: 10px;
  margin-inline-start: -9px;
}
.doorHangerRight:before {
  inset-inline-end: 10px;
  margin-inline-end: -9px;
}

#findResultsCount {
  background-color: rgba(217, 217, 217, 1);
  color: rgba(82, 82, 82, 1);
  text-align: center;
  padding: 4px 5px;
  margin: 5px;
}

#findMsg {
  color: rgba(251, 0, 0, 1);
}

#findResultsCount:empty,
#findMsg:empty {
  display: none;
}

#toolbarViewerMiddle {
  position: absolute;
  left: 50%;
  transform: translateX(-50%);
}

#toolbarViewerLeft,
#toolbarSidebarLeft {
  float: inline-start;
}
#toolbarViewerRight,
#toolbarSidebarRight {
  float: inline-end;
}

#toolbarViewerLeft > *,
#toolbarViewerMiddle > *,
#toolbarViewerRight > *,
#toolbarSidebarLeft *,
#toolbarSidebarRight *,
.findbar * {
  position: relative;
  float: inline-start;
}

#toolbarViewerLeft {
  padding-inline-start: 1px;
}
#toolbarViewerRight {
  padding-inline-end: 1px;
}
#toolbarSidebarRight {
  padding-inline-end: 2px;
}

.splitToolbarButton {
  margin: 2px;
  display: inline-block;
}
.splitToolbarButton > .toolbarButton {
  float: inline-start;
}

.toolbarButton,
.secondaryToolbarButton,
.dialogButton {
  border: none;
  background: none;
  width: 28px;
  height: 28px;
  outline: none;
}

.dialogButton:hover,
.dialogButton:focus-visible {
  background-color: var(--dialog-button-hover-bg-color);
}

.dialogButton:hover > span,
.dialogButton:focus-visible > span {
  color: var(--dialog-button-hover-color);
}

.toolbarButton > span {
  display: inline-block;
  width: 0;
  height: 0;
  overflow: hidden;
}

.toolbarButton[disabled],
.secondaryToolbarButton[disabled],
.dialogButton[disabled] {
  opacity: 0.5;
}

.splitToolbarButton > .toolbarButton:hover,
.splitToolbarButton > .toolbarButton:focus-visible,
.dropdownToolbarButton:hover {
  background-color: var(--button-hover-color);
}
.splitToolbarButton > .toolbarButton {
  position: relative;
  margin: 0;
}
#toolbarSidebar .splitToolbarButton > .toolbarButton {
  margin-inline-end: 2px;
}

.splitToolbarButtonSeparator {
  float: inline-start;
  margin: 4px 0;
  width: 1px;
  height: 20px;
  background-color: var(--separator-color);
}

.toolbarButton,
.dropdownToolbarButton,
.secondaryToolbarButton,
.dialogButton {
  min-width: 16px;
  margin: 2px 1px;
  padding: 2px 6px 0;
  border: none;
  border-radius: 2px;
  color: var(--main-color);
  font-size: 12px;
  line-height: 14px;
  user-select: none;
  cursor: default;
  box-sizing: border-box;
}

.toolbarButton:hover,
.toolbarButton:focus-visible {
  background-color: var(--button-hover-color);
}
.secondaryToolbarButton:hover,
.secondaryToolbarButton:focus-visible {
  background-color: var(--doorhanger-hover-bg-color);
  color: var(--doorhanger-hover-color);
}

.toolbarButton.toggled,
.splitToolbarButton.toggled > .toolbarButton.toggled,
.secondaryToolbarButton.toggled {
  background-color: var(--toggled-btn-bg-color);
  color: var(--toggled-btn-color);
}

.toolbarButton.toggled::before,
.secondaryToolbarButton.toggled::before {
  background-color: var(--toggled-btn-color);
}

.toolbarButton.toggled:hover:active,
.splitToolbarButton.toggled > .toolbarButton.toggled:hover:active,
.secondaryToolbarButton.toggled:hover:active {
  background-color: var(--toggled-hover-active-btn-color);
}

.dropdownToolbarButton {
  width: var(--scale-select-width);
  padding: 0;
  background-color: var(--dropdown-btn-bg-color);
  border: var(--dropdown-btn-border);
}
.dropdownToolbarButton::after {
  top: 6px;
  inset-inline-end: 6px;
  pointer-events: none;
  mask-image: var(--toolbarButton-menuArrow-icon);
}

.dropdownToolbarButton > select {
  appearance: none;
  width: inherit;
  height: 28px;
  font-size: 12px;
  color: var(--main-color);
  margin: 0;
  padding: 1px 0 2px;
  padding-inline-start: 6px;
  border: none;
  background-color: var(--dropdown-btn-bg-color);
}
.dropdownToolbarButton > select:hover,
.dropdownToolbarButton > select:focus-visible {
  background-color: var(--button-hover-color);
  color: var(--toggled-btn-color);
}
.dropdownToolbarButton > select > option {
  background: var(--doorhanger-bg-color);
  color: var(--main-color);
}

.toolbarButtonSpacer {
  width: 30px;
  display: inline-block;
  height: 1px;
}

.toolbarButton::before,
.secondaryToolbarButton::before,
.dropdownToolbarButton::after,
.treeItemToggler::before {
  /* All matching images have a size of 16x16
   * All relevant containers have a size of 28x28 */
  position: absolute;
  display: inline-block;
  width: 16px;
  height: 16px;

  content: "";
  background-color: var(--toolbar-icon-bg-color);
  mask-size: cover;
}

.dropdownToolbarButton:hover::after,
.dropdownToolbarButton:focus-visible::after,
.dropdownToolbarButton:active::after {
  background-color: var(--toolbar-icon-hover-bg-color);
}

.toolbarButton::before {
  opacity: var(--toolbar-icon-opacity);
  top: 6px;
  left: 6px;
}

.toolbarButton:hover::before,
.toolbarButton:focus-visible::before,
.secondaryToolbarButton:hover::before,
.secondaryToolbarButton:focus-visible::before {
  background-color: var(--toolbar-icon-hover-bg-color);
}

.secondaryToolbarButton::before {
  opacity: var(--doorhanger-icon-opacity);
  top: 5px;
  inset-inline-start: 12px;
}

#sidebarToggle::before {
  mask-image: var(--toolbarButton-sidebarToggle-icon);
  transform: scaleX(var(--dir-factor));
}

#secondaryToolbarToggle::before {
  mask-image: var(--toolbarButton-secondaryToolbarToggle-icon);
  transform: scaleX(var(--dir-factor));
}

#findPrevious::before {
  mask-image: var(--findbarButton-previous-icon);
}

#findNext::before {
  mask-image: var(--findbarButton-next-icon);
}

#previous::before {
  mask-image: var(--toolbarButton-pageUp-icon);
}

#next::before {
  mask-image: var(--toolbarButton-pageDown-icon);
}

#zoomOut::before {
  mask-image: var(--toolbarButton-zoomOut-icon);
}

#zoomIn::before {
  mask-image: var(--toolbarButton-zoomIn-icon);
}

#presentationMode::before {
  mask-image: var(--toolbarButton-presentationMode-icon);
}

#editorFreeText::before {
  mask-image: var(--toolbarButton-editorFreeText-icon);
}

#editorInk::before {
  mask-image: var(--toolbarButton-editorInk-icon);
}

#print::before,
#secondaryPrint::before {
  mask-image: var(--toolbarButton-print-icon);
}

#download::before,
#secondaryDownload::before {
  mask-image: var(--toolbarButton-download-icon);
}

a.secondaryToolbarButton {
  padding-top: 5px;
  text-decoration: none;
}
a.toolbarButton[href=""#""],
a.secondaryToolbarButton[href=""#""] {
  opacity: 0.5;
  pointer-events: none;
}

#viewBookmark::before {
  mask-image: var(--toolbarButton-bookmark-icon);
}

#viewThumbnail::before {
  mask-image: var(--toolbarButton-viewThumbnail-icon);
}

#viewOutline::before {
  mask-image: var(--toolbarButton-viewOutline-icon);
  transform: scaleX(var(--dir-factor));
}

#viewAttachments::before {
  mask-image: var(--toolbarButton-viewAttachments-icon);
}

#viewLayers::before {
  mask-image: var(--toolbarButton-viewLayers-icon);
}

#currentOutlineItem::before {
  mask-image: var(--toolbarButton-currentOutlineItem-icon);
  transform: scaleX(var(--dir-factor));
}

#viewFind::before {
  mask-image: var(--toolbarButton-search-icon);
}

.pdfSidebarNotification::after {
  position: absolute;
  display: inline-block;
  top: 2px;
  inset-inline-end: 2px;
  /* Create a filled circle, with a diameter of 9 pixels, using only CSS: */
  content: "";
  background-color: rgba(112, 219, 85, 1);
  height: 9px;
  width: 9px;
  border-radius: 50%;
}

.secondaryToolbarButton {
  position: relative;
  margin: 0;
  padding: 0 0 1px;
  padding-inline-start: 36px;
  height: auto;
  min-height: 26px;
  width: auto;
  min-width: 100%;
  text-align: start;
  white-space: normal;
  border-radius: 0;
  box-sizing: border-box;
  display: inline-block;
}
.secondaryToolbarButton > span {
  padding-inline-end: 4px;
}

#firstPage::before {
  mask-image: var(--secondaryToolbarButton-firstPage-icon);
}

#lastPage::before {
  mask-image: var(--secondaryToolbarButton-lastPage-icon);
}

#pageRotateCcw::before {
  mask-image: var(--secondaryToolbarButton-rotateCcw-icon);
}

#pageRotateCw::before {
  mask-image: var(--secondaryToolbarButton-rotateCw-icon);
}

#cursorSelectTool::before {
  mask-image: var(--secondaryToolbarButton-selectTool-icon);
}

#cursorHandTool::before {
  mask-image: var(--secondaryToolbarButton-handTool-icon);
}

#scrollPage::before {
  mask-image: var(--secondaryToolbarButton-scrollPage-icon);
}

#scrollVertical::before {
  mask-image: var(--secondaryToolbarButton-scrollVertical-icon);
}

#scrollHorizontal::before {
  mask-image: var(--secondaryToolbarButton-scrollHorizontal-icon);
}

#scrollWrapped::before {
  mask-image: var(--secondaryToolbarButton-scrollWrapped-icon);
}

#spreadNone::before {
  mask-image: var(--secondaryToolbarButton-spreadNone-icon);
}

#spreadOdd::before {
  mask-image: var(--secondaryToolbarButton-spreadOdd-icon);
}

#spreadEven::before {
  mask-image: var(--secondaryToolbarButton-spreadEven-icon);
}

#documentProperties::before {
  mask-image: var(--secondaryToolbarButton-documentProperties-icon);
}

.verticalToolbarSeparator {
  display: block;
  margin: 5px 2px;
  width: 1px;
  height: 22px;
  background-color: var(--separator-color);
}
.horizontalToolbarSeparator {
  display: block;
  margin: 6px 0;
  height: 1px;
  width: 100%;
  background-color: var(--doorhanger-separator-color);
}

.toolbarField {
  padding: 4px 7px;
  margin: 3px 0;
  border-radius: 2px;
  background-color: var(--field-bg-color);
  background-clip: padding-box;
  border: 1px solid var(--field-border-color);
  box-shadow: none;
  color: var(--field-color);
  font-size: 12px;
  line-height: 16px;
  outline: none;
}

.toolbarField[type=""checkbox""] {
  opacity: 0;
  position: absolute !important;
  left: 0;
  margin: 10px 0 3px;
  margin-inline-start: 7px;
}

#pageNumber {
  -moz-appearance: textfield; /* hides the spinner in moz */
  text-align: right;
  width: 40px;
}
#pageNumber.visiblePageIsLoading {
  background-image: var(--loading-icon);
  background-repeat: no-repeat;
  background-position: 3px;
}

.toolbarField:focus {
  border-color: #0a84ff;
}

.toolbarLabel {
  min-width: 16px;
  padding: 7px;
  margin: 2px;
  border-radius: 2px;
  color: var(--main-color);
  font-size: 12px;
  line-height: 14px;
  text-align: left;
  user-select: none;
  cursor: default;
}

#numPages.toolbarLabel {
  padding-inline-start: 3px;
}

#thumbnailView,
#outlineView,
#attachmentsView,
#layersView {
  position: absolute;
  width: calc(100% - 8px);
  top: 0;
  bottom: 0;
  padding: 4px 4px 0;
  overflow: auto;
  user-select: none;
}
#thumbnailView {
  width: calc(100% - 60px);
  padding: 10px 30px 0;
}

#thumbnailView > a:active,
#thumbnailView > a:focus {
  outline: 0;
}

.thumbnail {
  float: inline-start;
  margin: 0 10px 5px;
}

#thumbnailView > a:last-of-type > .thumbnail {
  margin-bottom: 10px;
}
#thumbnailView > a:last-of-type > .thumbnail:not([data-loaded]) {
  margin-bottom: 9px;
}

.thumbnail:not([data-loaded]) {
  border: 1px dashed rgba(132, 132, 132, 1);
  margin: -1px 9px 4px;
}

.thumbnailImage {
  border: 1px solid rgba(0, 0, 0, 0);
  box-shadow: 0 0 0 1px rgba(0, 0, 0, 0.5), 0 2px 8px rgba(0, 0, 0, 0.3);
  opacity: 0.8;
  z-index: 99;
  background-color: rgba(255, 255, 255, 1);
  background-clip: content-box;
}

.thumbnailSelectionRing {
  border-radius: 2px;
  padding: 7px;
}

a:focus > .thumbnail > .thumbnailSelectionRing > .thumbnailImage,
.thumbnail:hover > .thumbnailSelectionRing > .thumbnailImage {
  opacity: 0.9;
}

a:focus > .thumbnail > .thumbnailSelectionRing,
.thumbnail:hover > .thumbnailSelectionRing {
  background-color: var(--sidebaritem-bg-color);
  background-clip: padding-box;
  color: rgba(255, 255, 255, 0.9);
}

.thumbnail.selected > .thumbnailSelectionRing > .thumbnailImage {
  opacity: 1;
}

.thumbnail.selected > .thumbnailSelectionRing {
  background-color: var(--sidebaritem-bg-color);
  background-clip: padding-box;
  color: rgba(255, 255, 255, 1);
}

.treeWithDeepNesting > .treeItem,
.treeItem > .treeItems {
  margin-inline-start: 20px;
}

.treeItem > a {
  text-decoration: none;
  display: inline-block;
  /* Subtract the right padding (left, in RTL mode) of the container: */
  min-width: calc(100% - 4px);
  height: auto;
  margin-bottom: 1px;
  padding: 2px 0 5px;
  padding-inline-start: 4px;
  border-radius: 2px;
  color: var(--treeitem-color);
  font-size: 13px;
  line-height: 15px;
  user-select: none;
  white-space: normal;
  cursor: pointer;
}

#layersView .treeItem > a * {
  cursor: pointer;
}
#layersView .treeItem > a > label {
  padding-inline-start: 4px;
}
#layersView .treeItem > a > label > input {
  float: inline-start;
  margin-top: 1px;
}

.treeItemToggler {
  position: relative;
  float: inline-start;
  height: 0;
  width: 0;
  color: rgba(255, 255, 255, 0.5);
}
.treeItemToggler::before {
  inset-inline-end: 4px;
  mask-image: var(--treeitem-expanded-icon);
}
.treeItemToggler.treeItemsHidden::before {
  mask-image: var(--treeitem-collapsed-icon);
  transform: scaleX(var(--dir-factor));
}
.treeItemToggler.treeItemsHidden ~ .treeItems {
  display: none;
}

.treeItem.selected > a {
  background-color: var(--treeitem-selected-bg-color);
  color: var(--treeitem-selected-color);
}

.treeItemToggler:hover,
.treeItemToggler:hover + a,
.treeItemToggler:hover ~ .treeItems,
.treeItem > a:hover {
  background-color: var(--sidebaritem-bg-color);
  background-clip: padding-box;
  border-radius: 2px;
  color: var(--treeitem-hover-color);
}

.dialogButton {
  width: auto;
  margin: 3px 4px 2px !important;
  padding: 2px 11px;
  color: var(--main-color);
  background-color: var(--dialog-button-bg-color);
  border: var(--dialog-button-border) !important;
}

dialog {
  margin: auto;
  padding: 15px;
  border-spacing: 4px;
  color: var(--main-color);
  font: message-box;
  font-size: 12px;
  line-height: 14px;
  background-color: var(--doorhanger-bg-color);
  border: 1px solid rgba(0, 0, 0, 0.5);
  border-radius: 4px;
  box-shadow: 0 1px 4px rgba(0, 0, 0, 0.3);
}
dialog::backdrop {
  background-color: rgba(0, 0, 0, 0.2);
}

dialog > .row {
  display: table-row;
}

dialog > .row > * {
  display: table-cell;
}

dialog .toolbarField {
  margin: 5px 0;
}

dialog .separator {
  display: block;
  margin: 4px 0;
  height: 1px;
  width: 100%;
  background-color: var(--separator-color);
}

dialog .buttonRow {
  text-align: center;
  vertical-align: middle;
}

dialog :link {
  color: rgba(255, 255, 255, 1);
}

#passwordDialog {
  text-align: center;
}
#passwordDialog .toolbarField {
  width: 200px;
}

#documentPropertiesDialog {
  text-align: left;
}
#documentPropertiesDialog .row > * {
  min-width: 100px;
  text-align: start;
}
#documentPropertiesDialog .row > span {
  width: 125px;
  word-wrap: break-word;
}
#documentPropertiesDialog .row > p {
  max-width: 225px;
  word-wrap: break-word;
}
#documentPropertiesDialog .buttonRow {
  margin-top: 10px;
}

.grab-to-pan-grab {
  cursor: grab !important;
}
.grab-to-pan-grab
  *:not(input):not(textarea):not(button):not(select):not(:link) {
  cursor: inherit !important;
}
.grab-to-pan-grab:active,
.grab-to-pan-grabbing {
  cursor: grabbing !important;
  position: fixed;
  background: rgba(0, 0, 0, 0);
  display: block;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  overflow: hidden;
  z-index: 50000; /* should be higher than anything else in PDF.js! */
}

@page {
  margin: 0;
}

#printContainer {
  display: none;
}

@media print {
  body {
    background: rgba(0, 0, 0, 0) none;
  }
  body[data-pdfjsprinting] #outerContainer {
    display: none;
  }
  body[data-pdfjsprinting] #printContainer {
    display: block;
  }
  #printContainer {
    height: 100%;
  }
  /* wrapper around (scaled) print canvas elements */
  #printContainer > .printedPage {
    page-break-after: always;
    page-break-inside: avoid;

    /* The wrapper always cover the whole page. */
    height: 100%;
    width: 100%;

    display: flex;
    flex-direction: column;
    justify-content: center;
    align-items: center;
  }

  #printContainer > .xfaPrintedPage .xfaPage {
    position: absolute;
  }

  #printContainer > .xfaPrintedPage {
    page-break-after: always;
    page-break-inside: avoid;
    width: 100%;
    height: 100%;
    position: relative;
  }

  #printContainer > .printedPage canvas,
  #printContainer > .printedPage img {
    /* The intrinsic canvas / image size will make sure that we fit the page. */
    max-width: 100%;
    max-height: 100%;

    direction: ltr;
    display: block;
  }
}

.visibleLargeView,
.visibleMediumView {
  display: none;
}

@media all and (max-width: 900px) {
  #toolbarViewerMiddle {
    display: table;
    margin: auto;
    left: auto;
    position: inherit;
    transform: none;
  }
}

@media all and (max-width: 840px) {
  #sidebarContainer {
    background-color: var(--sidebar-narrow-bg-color);
  }
  #outerContainer.sidebarOpen #viewerContainer {
    inset-inline-start: 0 !important;
  }
}

@media all and (max-width: 820px) {
  #outerContainer .hiddenLargeView {
    display: none;
  }
  #outerContainer .visibleLargeView {
    display: inherit;
  }
}

@media all and (max-width: 750px) {
  #outerContainer .hiddenMediumView {
    display: none;
  }
  #outerContainer .visibleMediumView {
    display: inherit;
  }
}

@media all and (max-width: 690px) {
  .hiddenSmallView,
  .hiddenSmallView * {
    display: none;
  }
  .toolbarButtonSpacer {
    width: 0;
  }
  .findbar {
    inset-inline-start: 34px;
  }
}

@media all and (max-width: 560px) {
  #scaleSelectContainer {
    display: none;
  }
}

    </style>


<img src='"); sb.Append(rootPath); sb.Append(@"//print1.jpg' width='1000px'>
<div style=""width: 992px; height: 1403px;"" data-page-number=""1"" role=""region"" aria-label=""Page 1""
        data-loaded=""true"">
        <div class=""textLayer""
            style=""width: calc(var(--scale-factor) * 595.32001px); height: calc(var(--scale-factor) * 841.92004px);""
            data-main-rotation=""0""><span
                style=""left: 40.48%; top: 4.32%; font-size: calc(var(--scale-factor)*24.96px); font-family: serif;""
                role=""presentation"" dir=""ltr"">診</span><span
                style=""left: 44.67%; top: 4.32%; font-size: calc(var(--scale-factor)*24.96px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 48.86%; top: 4.32%; font-size: calc(var(--scale-factor)*24.96px); font-family: serif;""
                role=""presentation"" dir=""ltr"">療</span><span
                style=""left: 53.05%; top: 4.32%; font-size: calc(var(--scale-factor)*24.96px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 57.21%; top: 4.32%; font-size: calc(var(--scale-factor)*24.96px); font-family: serif;""
                role=""presentation"" dir=""ltr"">録</span><br role=""presentation""><span
                style=""left: 51.08%; top: 11.72%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif; transform: scaleX(1.04804);""
                role=""presentation"" dir=""ltr"">記号・番号</span><br role=""presentation""><span
                style=""left: 51.1%; top: 14.78%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif; transform: scaleX(1.1011);""
                role=""presentation"" dir=""ltr"">有 効 期 限</span><br role=""presentation""><span
                style=""left: 51.14%; top: 19.61%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif; transform: scaleX(1.09689);""
                role=""presentation"" dir=""ltr"">資 格 取 得</span><br role=""presentation""><span
                style=""left: 81.35%; top: 37.46%; font-size: calc(var(--scale-factor)*12.00px); font-family: serif; transform: scaleX(1.13899);""
                role=""presentation"" dir=""ltr"">期 間 満 了</span><br role=""presentation""><span
                style=""left: 51%; top: 17.37%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif; transform: scaleX(0.880036);""
                role=""presentation"" dir=""ltr"">被保険者氏名</span><br role=""presentation""><span
                style=""left: 32.82%; top: 33.34%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr"">被</span><span
                style=""left: 34.41%; top: 33.34%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr"">保</span><span
                style=""left: 36%; top: 33.34%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr"">険</span><span
                style=""left: 37.61%; top: 33.34%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr"">者</span><br role=""presentation""><span
                style=""left: 39.99%; top: 19.95%; font-size: calc(var(--scale-factor)*8.04px); font-family: serif;""
                role=""presentation"" dir=""ltr"">歳</span><br role=""presentation""><span
                style=""left: 9.88%; top: 78.65%; font-size: calc(var(--scale-factor)*9.96px); font-family: serif;""
                role=""presentation"" dir=""ltr"">傷</span><span
                style=""left: 11.55%; top: 78.65%; font-size: calc(var(--scale-factor)*9.96px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 19.26%; top: 78.65%; font-size: calc(var(--scale-factor)*9.96px); font-family: serif;""
                role=""presentation"" dir=""ltr"">病</span><span
                style=""left: 20.94%; top: 78.65%; font-size: calc(var(--scale-factor)*9.96px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 28.67%; top: 78.65%; font-size: calc(var(--scale-factor)*9.96px); font-family: serif;""
                role=""presentation"" dir=""ltr"">名</span><br role=""presentation""><span
                style=""left: 37.47%; top: 77.52%; font-size: calc(var(--scale-factor)*8.04px); font-family: serif;""
                role=""presentation"" dir=""ltr"">労</span><span
                style=""left: 38.82%; top: 77.52%; font-size: calc(var(--scale-factor)*8.04px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 40.92%; top: 77.52%; font-size: calc(var(--scale-factor)*8.04px); font-family: serif;""
                role=""presentation"" dir=""ltr"">務</span><span
                style=""left: 42.27%; top: 77.52%; font-size: calc(var(--scale-factor)*8.04px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 44.36%; top: 77.52%; font-size: calc(var(--scale-factor)*8.04px); font-family: serif;""
                role=""presentation"" dir=""ltr"">不</span><span
                style=""left: 45.71%; top: 77.52%; font-size: calc(var(--scale-factor)*8.04px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 47.82%; top: 77.52%; font-size: calc(var(--scale-factor)*8.04px); font-family: serif;""
                role=""presentation"" dir=""ltr"">能</span><span
                style=""left: 49.17%; top: 77.52%; font-size: calc(var(--scale-factor)*8.04px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 51.26%; top: 77.52%; font-size: calc(var(--scale-factor)*8.04px); font-family: serif;""
                role=""presentation"" dir=""ltr"">に</span><span
                style=""left: 52.61%; top: 77.52%; font-size: calc(var(--scale-factor)*8.04px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 54.71%; top: 77.52%; font-size: calc(var(--scale-factor)*8.04px); font-family: serif;""
                role=""presentation"" dir=""ltr"">関</span><span
                style=""left: 56.06%; top: 77.52%; font-size: calc(var(--scale-factor)*8.04px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 58.18%; top: 77.52%; font-size: calc(var(--scale-factor)*8.04px); font-family: serif;""
                role=""presentation"" dir=""ltr"">す</span><span
                style=""left: 59.53%; top: 77.52%; font-size: calc(var(--scale-factor)*8.04px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 61.57%; top: 77.52%; font-size: calc(var(--scale-factor)*8.04px); font-family: serif;""
                role=""presentation"" dir=""ltr"">る</span><span
                style=""left: 62.92%; top: 77.52%; font-size: calc(var(--scale-factor)*8.04px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 65.01%; top: 77.52%; font-size: calc(var(--scale-factor)*8.04px); font-family: serif;""
                role=""presentation"" dir=""ltr"">意</span><span
                style=""left: 66.36%; top: 77.52%; font-size: calc(var(--scale-factor)*8.04px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 68.45%; top: 77.52%; font-size: calc(var(--scale-factor)*8.04px); font-family: serif;""
                role=""presentation"" dir=""ltr"">見</span><br role=""presentation""><span
                style=""left: 35.07%; top: 78.79%; font-size: calc(var(--scale-factor)*8.04px); font-family: serif;""
                role=""presentation"" dir=""ltr"">意</span><span
                style=""left: 36.42%; top: 78.79%; font-size: calc(var(--scale-factor)*8.04px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 37.44%; top: 78.79%; font-size: calc(var(--scale-factor)*8.04px); font-family: serif;""
                role=""presentation"" dir=""ltr"">見</span><span
                style=""left: 38.79%; top: 78.79%; font-size: calc(var(--scale-factor)*8.04px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 39.81%; top: 78.79%; font-size: calc(var(--scale-factor)*8.04px); font-family: serif;""
                role=""presentation"" dir=""ltr"">書</span><span
                style=""left: 41.16%; top: 78.79%; font-size: calc(var(--scale-factor)*8.04px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 42.19%; top: 78.79%; font-size: calc(var(--scale-factor)*8.04px); font-family: serif;""
                role=""presentation"" dir=""ltr"">に</span><span
                style=""left: 43.54%; top: 78.79%; font-size: calc(var(--scale-factor)*8.04px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 44.57%; top: 78.79%; font-size: calc(var(--scale-factor)*8.04px); font-family: serif;""
                role=""presentation"" dir=""ltr"">記</span><span
                style=""left: 45.92%; top: 78.79%; font-size: calc(var(--scale-factor)*8.04px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 46.95%; top: 78.79%; font-size: calc(var(--scale-factor)*8.04px); font-family: serif;""
                role=""presentation"" dir=""ltr"">入</span><span
                style=""left: 48.3%; top: 78.79%; font-size: calc(var(--scale-factor)*8.04px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 49.32%; top: 78.79%; font-size: calc(var(--scale-factor)*8.04px); font-family: serif;""
                role=""presentation"" dir=""ltr"">し</span><span
                style=""left: 50.67%; top: 78.79%; font-size: calc(var(--scale-factor)*8.04px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 51.7%; top: 78.79%; font-size: calc(var(--scale-factor)*8.04px); font-family: serif;""
                role=""presentation"" dir=""ltr"">た</span><span
                style=""left: 53.05%; top: 78.79%; font-size: calc(var(--scale-factor)*8.04px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 58.54%; top: 79.5%; font-size: calc(var(--scale-factor)*8.04px); font-family: serif;""
                role=""presentation"" dir=""ltr"">意</span><span
                style=""left: 59.89%; top: 79.5%; font-size: calc(var(--scale-factor)*8.04px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 60.99%; top: 79.5%; font-size: calc(var(--scale-factor)*8.04px); font-family: serif;""
                role=""presentation"" dir=""ltr"">見</span><span
                style=""left: 62.35%; top: 79.5%; font-size: calc(var(--scale-factor)*8.04px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 63.45%; top: 79.5%; font-size: calc(var(--scale-factor)*8.04px); font-family: serif;""
                role=""presentation"" dir=""ltr"">書</span><span
                style=""left: 64.8%; top: 79.5%; font-size: calc(var(--scale-factor)*8.04px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 65.91%; top: 79.5%; font-size: calc(var(--scale-factor)*8.04px); font-family: serif;""
                role=""presentation"" dir=""ltr"">交</span><span
                style=""left: 67.26%; top: 79.5%; font-size: calc(var(--scale-factor)*8.04px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 68.37%; top: 79.5%; font-size: calc(var(--scale-factor)*8.04px); font-family: serif;""
                role=""presentation"" dir=""ltr"">付</span><span
                style=""left: 69.72%; top: 79.5%; font-size: calc(var(--scale-factor)*8.04px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 76.23%; top: 78.69%; font-size: calc(var(--scale-factor)*9.96px); font-family: serif;""
                role=""presentation"" dir=""ltr"">入</span><span
                style=""left: 77.91%; top: 78.69%; font-size: calc(var(--scale-factor)*9.96px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 80.89%; top: 78.69%; font-size: calc(var(--scale-factor)*9.96px); font-family: serif;""
                role=""presentation"" dir=""ltr"">院</span><span
                style=""left: 82.56%; top: 78.69%; font-size: calc(var(--scale-factor)*9.96px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 85.55%; top: 78.69%; font-size: calc(var(--scale-factor)*9.96px); font-family: serif;""
                role=""presentation"" dir=""ltr"">期</span><span
                style=""left: 87.23%; top: 78.69%; font-size: calc(var(--scale-factor)*9.96px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 90.22%; top: 78.69%; font-size: calc(var(--scale-factor)*9.96px); font-family: serif;""
                role=""presentation"" dir=""ltr"">間</span><br role=""presentation""><span
                style=""left: 34.71%; top: 82.81%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif;""
                role=""presentation"" dir=""ltr"">至</span><span
                style=""left: 36.22%; top: 82.81%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 39.08%; top: 82.81%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif;""
                role=""presentation"" dir=""ltr"">年</span><span
                style=""left: 40.59%; top: 82.81%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 43.48%; top: 82.81%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif;""
                role=""presentation"" dir=""ltr"">月</span><span
                style=""left: 44.99%; top: 82.81%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 47.85%; top: 82.81%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif;""
                role=""presentation"" dir=""ltr"">日</span><br role=""presentation""><span
                style=""left: 34.71%; top: 81.57%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif;""
                role=""presentation"" dir=""ltr"">自</span><span
                style=""left: 36.22%; top: 81.57%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 39.08%; top: 81.57%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif;""
                role=""presentation"" dir=""ltr"">年</span><span
                style=""left: 40.59%; top: 81.57%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 43.48%; top: 81.57%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif;""
                role=""presentation"" dir=""ltr"">月</span><span
                style=""left: 44.99%; top: 81.57%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 47.85%; top: 81.57%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif;""
                role=""presentation"" dir=""ltr"">日</span><span
                style=""left: 49.36%; top: 81.57%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 52.35%; top: 82.24%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif; transform: scaleX(0.96504);""
                role=""presentation"" dir=""ltr"">日間</span><span
                style=""left: 55.27%; top: 82.24%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 60.87%; top: 82.25%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif;""
                role=""presentation"" dir=""ltr"">年</span><span
                style=""left: 62.39%; top: 82.25%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 64.79%; top: 82.25%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif;""
                role=""presentation"" dir=""ltr"">月</span><span
                style=""left: 66.3%; top: 82.25%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 68.72%; top: 82.25%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif;""
                role=""presentation"" dir=""ltr"">日</span><span
                style=""left: 70.23%; top: 82.25%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 74.42%; top: 81.81%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif;""
                role=""presentation"" dir=""ltr"">自</span><span
                style=""left: 75.93%; top: 81.81%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 78.59%; top: 81.81%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif;""
                role=""presentation"" dir=""ltr"">年</span><span
                style=""left: 80.11%; top: 81.81%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 82.77%; top: 81.81%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif;""
                role=""presentation"" dir=""ltr"">月</span><span
                style=""left: 84.28%; top: 81.81%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 86.94%; top: 81.81%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif;""
                role=""presentation"" dir=""ltr"">日</span><br role=""presentation""><span
                style=""left: 55.23%; top: 22.73%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif; transform: scaleX(1.03004);""
                role=""presentation"" dir=""ltr"">所在地</span><br role=""presentation""><span
                style=""left: 74.42%; top: 83.06%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif;""
                role=""presentation"" dir=""ltr"">至</span><span
                style=""left: 75.93%; top: 83.06%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 78.59%; top: 83.06%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif;""
                role=""presentation"" dir=""ltr"">年</span><span
                style=""left: 80.11%; top: 83.06%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 82.77%; top: 83.06%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif;""
                role=""presentation"" dir=""ltr"">月</span><span
                style=""left: 84.28%; top: 83.06%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 86.94%; top: 83.06%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif;""
                role=""presentation"" dir=""ltr"">日</span><span
                style=""left: 88.45%; top: 83.06%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 90.53%; top: 82.38%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif; transform: scaleX(1.01504);""
                role=""presentation"" dir=""ltr"">日間</span><br role=""presentation""><span
                style=""left: 74.42%; top: 84.89%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif;""
                role=""presentation"" dir=""ltr"">自</span><span
                style=""left: 75.93%; top: 84.89%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 78.59%; top: 84.89%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif;""
                role=""presentation"" dir=""ltr"">年</span><span
                style=""left: 80.11%; top: 84.89%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 82.77%; top: 84.89%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif;""
                role=""presentation"" dir=""ltr"">月</span><span
                style=""left: 84.28%; top: 84.89%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 86.94%; top: 84.89%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif;""
                role=""presentation"" dir=""ltr"">日</span><br role=""presentation""><span
                style=""left: 74.42%; top: 86.27%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif;""
                role=""presentation"" dir=""ltr"">至</span><span
                style=""left: 75.93%; top: 86.27%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 78.59%; top: 86.27%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif;""
                role=""presentation"" dir=""ltr"">年</span><span
                style=""left: 80.11%; top: 86.27%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 82.77%; top: 86.27%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif;""
                role=""presentation"" dir=""ltr"">月</span><span
                style=""left: 84.28%; top: 86.27%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 86.94%; top: 86.27%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif;""
                role=""presentation"" dir=""ltr"">日</span><span
                style=""left: 88.45%; top: 86.27%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 90.51%; top: 85.67%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif; transform: scaleX(1.01504);""
                role=""presentation"" dir=""ltr"">日間</span><span
                style=""left: 60.83%; top: 85.57%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif;""
                role=""presentation"" dir=""ltr"">年</span><span
                style=""left: 62.35%; top: 85.57%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 64.77%; top: 85.57%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif;""
                role=""presentation"" dir=""ltr"">月</span><span
                style=""left: 66.28%; top: 85.57%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 68.7%; top: 85.57%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif;""
                role=""presentation"" dir=""ltr"">日</span><span
                style=""left: 52.33%; top: 85.42%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif; transform: scaleX(0.96504);""
                role=""presentation"" dir=""ltr"">日間</span><br role=""presentation""><span
                style=""left: 34.71%; top: 86.04%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif;""
                role=""presentation"" dir=""ltr"">至</span><span
                style=""left: 36.22%; top: 86.04%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 39.08%; top: 86.04%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif;""
                role=""presentation"" dir=""ltr"">年</span><span
                style=""left: 40.59%; top: 86.04%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 43.48%; top: 86.04%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif;""
                role=""presentation"" dir=""ltr"">月</span><span
                style=""left: 44.99%; top: 86.04%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 47.85%; top: 86.04%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif;""
                role=""presentation"" dir=""ltr"">日</span><br role=""presentation""><span
                style=""left: 34.71%; top: 84.76%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif;""
                role=""presentation"" dir=""ltr"">自</span><span
                style=""left: 36.22%; top: 84.76%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 39.08%; top: 84.76%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif;""
                role=""presentation"" dir=""ltr"">年</span><span
                style=""left: 40.59%; top: 84.76%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 43.48%; top: 84.76%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif;""
                role=""presentation"" dir=""ltr"">月</span><span
                style=""left: 44.99%; top: 84.76%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 47.85%; top: 84.76%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif;""
                role=""presentation"" dir=""ltr"">日</span><br role=""presentation""><span
                style=""left: 55.23%; top: 28.35%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr"">名</span><span
                style=""left: 57.09%; top: 28.35%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 59.11%; top: 28.35%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr"">称</span><br role=""presentation""><span
                style=""left: 55.13%; top: 26.04%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr"">電</span><span
                style=""left: 56.98%; top: 26.04%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 59.01%; top: 26.04%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr"">話</span><br role=""presentation""><span
                style=""left: 55.29%; top: 32.98%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr"">電</span><span
                style=""left: 57.15%; top: 32.98%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 59.17%; top: 32.98%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr"">話</span><br role=""presentation""><span
                style=""left: 56.8%; top: 89.11%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif; transform: scaleX(1.00718);""
                role=""presentation"" dir=""ltr"">公費負担者番号</span><br role=""presentation""><span
                style=""left: 55.29%; top: 30.73%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif; transform: scaleX(1.03004);""
                role=""presentation"" dir=""ltr"">所在地</span><br role=""presentation""><span
                style=""left: 56.8%; top: 92.01%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif; transform: scaleX(0.98364);""
                role=""presentation"" dir=""ltr"">公 費 負 担 医 療</span><br role=""presentation""><span
                style=""left: 56.88%; top: 93.17%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif; transform: scaleX(0.965705);""
                role=""presentation"" dir=""ltr"">の 受 給 者 番 号</span><br role=""presentation""><span
                style=""left: 32.82%; top: 34.89%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr"">と</span><span
                style=""left: 34.41%; top: 34.89%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr"">の</span><span
                style=""left: 36%; top: 34.89%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr"">続</span><span
                style=""left: 37.61%; top: 34.89%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr"">柄</span><br role=""presentation""><span
                style=""left: 8.47%; top: 6.46%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr"">患</span><span
                style=""left: 10.32%; top: 6.46%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 11.45%; top: 6.46%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr"">者</span><span
                style=""left: 13.31%; top: 6.46%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 14.46%; top: 6.46%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr"">番</span><span
                style=""left: 16.31%; top: 6.46%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 17.44%; top: 6.46%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr"">号</span><br role=""presentation""><span
                style=""left: 55.17%; top: 35.23%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr"">名</span><span
                style=""left: 57.02%; top: 35.23%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 59.05%; top: 35.23%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr"">称</span><br role=""presentation""><span
                style=""left: 33.94%; top: 44.68%; font-size: calc(var(--scale-factor)*9.96px); font-family: serif;""
                role=""presentation"" dir=""ltr"">上</span><br role=""presentation""><span
                style=""left: 33.92%; top: 45.42%; font-size: calc(var(--scale-factor)*9.96px); font-family: serif;""
                role=""presentation"" dir=""ltr"">・</span><br role=""presentation""><span
                style=""left: 33.92%; top: 46.18%; font-size: calc(var(--scale-factor)*9.96px); font-family: serif;""
                role=""presentation"" dir=""ltr"">外</span><br role=""presentation""><span
                style=""left: 33.94%; top: 48.33%; font-size: calc(var(--scale-factor)*9.96px); font-family: serif;""
                role=""presentation"" dir=""ltr"">上</span><br role=""presentation""><span
                style=""left: 33.92%; top: 49.07%; font-size: calc(var(--scale-factor)*9.96px); font-family: serif;""
                role=""presentation"" dir=""ltr"">・</span><br role=""presentation""><span
                style=""left: 33.92%; top: 49.83%; font-size: calc(var(--scale-factor)*9.96px); font-family: serif;""
                role=""presentation"" dir=""ltr"">外</span><br role=""presentation""><span
                style=""left: 33.94%; top: 52.02%; font-size: calc(var(--scale-factor)*9.96px); font-family: serif;""
                role=""presentation"" dir=""ltr"">上</span><br role=""presentation""><span
                style=""left: 33.92%; top: 52.76%; font-size: calc(var(--scale-factor)*9.96px); font-family: serif;""
                role=""presentation"" dir=""ltr"">・</span><br role=""presentation""><span
                style=""left: 33.92%; top: 53.52%; font-size: calc(var(--scale-factor)*9.96px); font-family: serif;""
                role=""presentation"" dir=""ltr"">外</span><br role=""presentation""><span
                style=""left: 33.94%; top: 55.58%; font-size: calc(var(--scale-factor)*9.96px); font-family: serif;""
                role=""presentation"" dir=""ltr"">上</span><br role=""presentation""><span
                style=""left: 33.92%; top: 56.32%; font-size: calc(var(--scale-factor)*9.96px); font-family: serif;""
                role=""presentation"" dir=""ltr"">・</span><br role=""presentation""><span
                style=""left: 33.92%; top: 57.08%; font-size: calc(var(--scale-factor)*9.96px); font-family: serif;""
                role=""presentation"" dir=""ltr"">外</span><br role=""presentation""><span
                style=""left: 7.88%; top: 9.21%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif; transform: scaleX(0.874321);""
                role=""presentation"" dir=""ltr"">公費負担者番号</span><br role=""presentation""><span
                style=""left: 33.94%; top: 59.25%; font-size: calc(var(--scale-factor)*9.96px); font-family: serif;""
                role=""presentation"" dir=""ltr"">上</span><br role=""presentation""><span
                style=""left: 33.92%; top: 59.99%; font-size: calc(var(--scale-factor)*9.96px); font-family: serif;""
                role=""presentation"" dir=""ltr"">・</span><br role=""presentation""><span
                style=""left: 33.92%; top: 60.74%; font-size: calc(var(--scale-factor)*9.96px); font-family: serif;""
                role=""presentation"" dir=""ltr"">外</span><br role=""presentation""><span
                style=""left: 33.94%; top: 62.85%; font-size: calc(var(--scale-factor)*9.96px); font-family: serif;""
                role=""presentation"" dir=""ltr"">上</span><br role=""presentation""><span
                style=""left: 33.92%; top: 63.59%; font-size: calc(var(--scale-factor)*9.96px); font-family: serif;""
                role=""presentation"" dir=""ltr"">・</span><br role=""presentation""><span
                style=""left: 33.92%; top: 64.35%; font-size: calc(var(--scale-factor)*9.96px); font-family: serif;""
                role=""presentation"" dir=""ltr"">外</span><br role=""presentation""><span
                style=""left: 91.63%; top: 41.27%; font-size: calc(var(--scale-factor)*9.96px); font-family: serif;""
                role=""presentation"" dir=""ltr"">年</span><br role=""presentation""><span
                style=""left: 84.08%; top: 42.78%; font-size: calc(var(--scale-factor)*9.96px); font-family: serif;""
                role=""presentation"" dir=""ltr"">月</span><span
                style=""left: 85.75%; top: 42.78%; font-size: calc(var(--scale-factor)*9.96px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 91.64%; top: 42.78%; font-size: calc(var(--scale-factor)*9.96px); font-family: serif;""
                role=""presentation"" dir=""ltr"">日</span><br role=""presentation""><span
                style=""left: 91.63%; top: 44.85%; font-size: calc(var(--scale-factor)*9.96px); font-family: serif;""
                role=""presentation"" dir=""ltr"">年</span><br role=""presentation""><span
                style=""left: 84.08%; top: 46.36%; font-size: calc(var(--scale-factor)*9.96px); font-family: serif;""
                role=""presentation"" dir=""ltr"">月</span><span
                style=""left: 85.75%; top: 46.36%; font-size: calc(var(--scale-factor)*9.96px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 91.64%; top: 46.36%; font-size: calc(var(--scale-factor)*9.96px); font-family: serif;""
                role=""presentation"" dir=""ltr"">日</span><br role=""presentation""><span
                style=""left: 91.63%; top: 48.43%; font-size: calc(var(--scale-factor)*9.96px); font-family: serif;""
                role=""presentation"" dir=""ltr"">年</span><br role=""presentation""><span
                style=""left: 84.08%; top: 49.95%; font-size: calc(var(--scale-factor)*9.96px); font-family: serif;""
                role=""presentation"" dir=""ltr"">月</span><span
                style=""left: 85.75%; top: 49.95%; font-size: calc(var(--scale-factor)*9.96px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 91.64%; top: 49.95%; font-size: calc(var(--scale-factor)*9.96px); font-family: serif;""
                role=""presentation"" dir=""ltr"">日</span><br role=""presentation""><span
                style=""left: 91.63%; top: 51.98%; font-size: calc(var(--scale-factor)*9.96px); font-family: serif;""
                role=""presentation"" dir=""ltr"">年</span><br role=""presentation""><span
                style=""left: 84.08%; top: 53.49%; font-size: calc(var(--scale-factor)*9.96px); font-family: serif;""
                role=""presentation"" dir=""ltr"">月</span><span
                style=""left: 85.75%; top: 53.49%; font-size: calc(var(--scale-factor)*9.96px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 91.64%; top: 53.49%; font-size: calc(var(--scale-factor)*9.96px); font-family: serif;""
                role=""presentation"" dir=""ltr"">日</span><br role=""presentation""><span
                style=""left: 91.63%; top: 55.74%; font-size: calc(var(--scale-factor)*9.96px); font-family: serif;""
                role=""presentation"" dir=""ltr"">年</span><br role=""presentation""><span
                style=""left: 84.08%; top: 57.25%; font-size: calc(var(--scale-factor)*9.96px); font-family: serif;""
                role=""presentation"" dir=""ltr"">月</span><span
                style=""left: 85.75%; top: 57.25%; font-size: calc(var(--scale-factor)*9.96px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 91.64%; top: 57.25%; font-size: calc(var(--scale-factor)*9.96px); font-family: serif;""
                role=""presentation"" dir=""ltr"">日</span><br role=""presentation""><span
                style=""left: 91.63%; top: 59.4%; font-size: calc(var(--scale-factor)*9.96px); font-family: serif;""
                role=""presentation"" dir=""ltr"">年</span><br role=""presentation""><span
                style=""left: 84.08%; top: 60.93%; font-size: calc(var(--scale-factor)*9.96px); font-family: serif;""
                role=""presentation"" dir=""ltr"">月</span><span
                style=""left: 85.75%; top: 60.93%; font-size: calc(var(--scale-factor)*9.96px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 91.64%; top: 60.93%; font-size: calc(var(--scale-factor)*9.96px); font-family: serif;""
                role=""presentation"" dir=""ltr"">日</span><br role=""presentation""><span
                style=""left: 91.63%; top: 62.95%; font-size: calc(var(--scale-factor)*9.96px); font-family: serif;""
                role=""presentation"" dir=""ltr"">年</span><br role=""presentation""><span
                style=""left: 84.08%; top: 64.46%; font-size: calc(var(--scale-factor)*9.96px); font-family: serif;""
                role=""presentation"" dir=""ltr"">月</span><span
                style=""left: 85.75%; top: 64.46%; font-size: calc(var(--scale-factor)*9.96px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 91.64%; top: 64.46%; font-size: calc(var(--scale-factor)*9.96px); font-family: serif;""
                role=""presentation"" dir=""ltr"">日</span><br role=""presentation""><span
                style=""left: 7.96%; top: 11.37%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif; transform: scaleX(1.02779);""
                role=""presentation"" dir=""ltr"">公 費 負 担 医 療</span><br role=""presentation""><span
                style=""left: 33.94%; top: 66.49%; font-size: calc(var(--scale-factor)*9.96px); font-family: serif;""
                role=""presentation"" dir=""ltr"">上</span><br role=""presentation""><span
                style=""left: 33.92%; top: 67.23%; font-size: calc(var(--scale-factor)*9.96px); font-family: serif;""
                role=""presentation"" dir=""ltr"">・</span><br role=""presentation""><span
                style=""left: 33.92%; top: 67.98%; font-size: calc(var(--scale-factor)*9.96px); font-family: serif;""
                role=""presentation"" dir=""ltr"">外</span><br role=""presentation""><span
                style=""left: 7.82%; top: 12.52%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif; transform: scaleX(1.04158);""
                role=""presentation"" dir=""ltr"">の 受 給 者 番 号</span><br role=""presentation""><span
                style=""left: 91.59%; top: 66.59%; font-size: calc(var(--scale-factor)*9.96px); font-family: serif;""
                role=""presentation"" dir=""ltr"">年</span><br role=""presentation""><span
                style=""left: 84.04%; top: 68.1%; font-size: calc(var(--scale-factor)*9.96px); font-family: serif;""
                role=""presentation"" dir=""ltr"">月</span><span
                style=""left: 85.71%; top: 68.1%; font-size: calc(var(--scale-factor)*9.96px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 91.6%; top: 68.1%; font-size: calc(var(--scale-factor)*9.96px); font-family: serif;""
                role=""presentation"" dir=""ltr"">日</span><br role=""presentation""><span
                style=""left: 33.94%; top: 70.06%; font-size: calc(var(--scale-factor)*9.96px); font-family: serif;""
                role=""presentation"" dir=""ltr"">上</span><br role=""presentation""><span
                style=""left: 33.92%; top: 70.81%; font-size: calc(var(--scale-factor)*9.96px); font-family: serif;""
                role=""presentation"" dir=""ltr"">・</span><br role=""presentation""><span
                style=""left: 33.92%; top: 71.56%; font-size: calc(var(--scale-factor)*9.96px); font-family: serif;""
                role=""presentation"" dir=""ltr"">外</span><br role=""presentation""><span
                style=""left: 91.57%; top: 70.31%; font-size: calc(var(--scale-factor)*9.96px); font-family: serif;""
                role=""presentation"" dir=""ltr"">年</span><br role=""presentation""><span
                style=""left: 84.02%; top: 71.82%; font-size: calc(var(--scale-factor)*9.96px); font-family: serif;""
                role=""presentation"" dir=""ltr"">月</span><span
                style=""left: 85.69%; top: 71.82%; font-size: calc(var(--scale-factor)*9.96px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 91.58%; top: 71.82%; font-size: calc(var(--scale-factor)*9.96px); font-family: serif;""
                role=""presentation"" dir=""ltr"">日</span><br role=""presentation""><span
                style=""left: 33.94%; top: 73.76%; font-size: calc(var(--scale-factor)*9.96px); font-family: serif;""
                role=""presentation"" dir=""ltr"">上</span><br role=""presentation""><span
                style=""left: 33.92%; top: 74.5%; font-size: calc(var(--scale-factor)*9.96px); font-family: serif;""
                role=""presentation"" dir=""ltr"">・</span><br role=""presentation""><span
                style=""left: 33.92%; top: 75.25%; font-size: calc(var(--scale-factor)*9.96px); font-family: serif;""
                role=""presentation"" dir=""ltr"">外</span><br role=""presentation""><span
                style=""left: 91.55%; top: 73.94%; font-size: calc(var(--scale-factor)*9.96px); font-family: serif;""
                role=""presentation"" dir=""ltr"">年</span><br role=""presentation""><span
                style=""left: 84%; top: 75.45%; font-size: calc(var(--scale-factor)*9.96px); font-family: serif;""
                role=""presentation"" dir=""ltr"">月</span><span
                style=""left: 85.67%; top: 75.45%; font-size: calc(var(--scale-factor)*9.96px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 91.56%; top: 75.45%; font-size: calc(var(--scale-factor)*9.96px); font-family: serif;""
                role=""presentation"" dir=""ltr"">日</span><br role=""presentation""><span
                style=""left: 8.2%; top: 16.28%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr"">受</span><br role=""presentation""><span
                style=""left: 8.2%; top: 24.69%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr"">診</span><br role=""presentation""><span
                style=""left: 8.2%; top: 33.11%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr"">者</span><br role=""presentation""><span
                style=""left: 11.15%; top: 30.6%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif; transform: scaleX(0.866035);""
                role=""presentation"" dir=""ltr"">緊急連絡先</span><br role=""presentation""><span
                style=""left: 52.79%; top: 22.33%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif;""
                role=""presentation"" dir=""ltr"">船</span><br role=""presentation""><span
                style=""left: 52.77%; top: 23.84%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif;""
                role=""presentation"" dir=""ltr"">舶</span><br role=""presentation""><span
                style=""left: 52.79%; top: 25.34%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif;""
                role=""presentation"" dir=""ltr"">所</span><br role=""presentation""><span
                style=""left: 52.77%; top: 26.85%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif;""
                role=""presentation"" dir=""ltr"">有</span><br role=""presentation""><span
                style=""left: 52.77%; top: 28.36%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif;""
                role=""presentation"" dir=""ltr"">者</span><br role=""presentation""><span
                style=""left: 51.08%; top: 22.8%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif;""
                role=""presentation"" dir=""ltr"">事</span><br role=""presentation""><span
                style=""left: 51.06%; top: 25.37%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif;""
                role=""presentation"" dir=""ltr"">業</span><br role=""presentation""><span
                style=""left: 51.08%; top: 27.92%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif;""
                role=""presentation"" dir=""ltr"">所</span><br role=""presentation""><span
                style=""left: 11.21%; top: 15.81%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr"">氏</span><span
                style=""left: 13.06%; top: 15.81%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 17.18%; top: 15.81%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr"">名</span><br role=""presentation""><span
                style=""left: 51.7%; top: 31.41%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr"">保</span><br role=""presentation""><span
                style=""left: 51.7%; top: 33.1%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr"">険</span><br role=""presentation""><span
                style=""left: 51.68%; top: 34.78%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr"">者</span><br role=""presentation""><span
                style=""left: 7.9%; top: 88.78%; font-size: calc(var(--scale-factor)*8.04px); font-family: serif; transform: scaleX(0.95549);""
                role=""presentation"" dir=""ltr"">業務災害又は通勤災害の</span><br role=""presentation""><span
                style=""left: 42.37%; top: 19.54%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif; transform: scaleX(1.02504);""
                role=""presentation"" dir=""ltr"">性別</span><span
                style=""left: 11.17%; top: 19.54%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif; transform: scaleX(1.07004);""
                role=""presentation"" dir=""ltr"">生年月日</span><br role=""presentation""><span
                style=""left: 11.27%; top: 24.1%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr"">住</span><span
                style=""left: 13.12%; top: 24.1%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 17.11%; top: 24.1%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr"">所</span><br role=""presentation""><span
                style=""left: 20%; top: 21.75%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr"">〒</span><br role=""presentation""><span
                style=""left: 11.17%; top: 28.39%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif; transform: scaleX(1.07004);""
                role=""presentation"" dir=""ltr"">電話番号</span><br role=""presentation""><span
                style=""left: 10.48%; top: 38.11%; font-size: calc(var(--scale-factor)*12.00px); font-family: serif;""
                role=""presentation"" dir=""ltr"">傷</span><span
                style=""left: 12.5%; top: 38.11%; font-size: calc(var(--scale-factor)*12.00px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 18.75%; top: 38.11%; font-size: calc(var(--scale-factor)*12.00px); font-family: serif;""
                role=""presentation"" dir=""ltr"">病</span><span
                style=""left: 20.76%; top: 38.11%; font-size: calc(var(--scale-factor)*12.00px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 27.03%; top: 38.11%; font-size: calc(var(--scale-factor)*12.00px); font-family: serif;""
                role=""presentation"" dir=""ltr"">名</span><span
                style=""left: 29.05%; top: 38.11%; font-size: calc(var(--scale-factor)*12.00px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 34.03%; top: 38.95%; font-size: calc(var(--scale-factor)*12.00px); font-family: serif;""
                role=""presentation"" dir=""ltr"">務</span><br role=""presentation""><span
                style=""left: 11.17%; top: 34.25%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr"">職</span><span
                style=""left: 13.02%; top: 34.25%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 17.12%; top: 34.25%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr"">業</span><br role=""presentation""><span
                style=""left: 33.94%; top: 41.06%; font-size: calc(var(--scale-factor)*9.96px); font-family: serif;""
                role=""presentation"" dir=""ltr"">上</span><br role=""presentation""><span
                style=""left: 33.92%; top: 41.8%; font-size: calc(var(--scale-factor)*9.96px); font-family: serif;""
                role=""presentation"" dir=""ltr"">・</span><br role=""presentation""><span
                style=""left: 33.92%; top: 42.56%; font-size: calc(var(--scale-factor)*9.96px); font-family: serif;""
                role=""presentation"" dir=""ltr"">外</span><br role=""presentation""><span
                style=""left: 39.04%; top: 38.08%; font-size: calc(var(--scale-factor)*12.00px); font-family: serif;""
                role=""presentation"" dir=""ltr"">開</span><span
                style=""left: 41.06%; top: 38.08%; font-size: calc(var(--scale-factor)*12.00px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 46.06%; top: 38.08%; font-size: calc(var(--scale-factor)*12.00px); font-family: serif;""
                role=""presentation"" dir=""ltr"">始</span><span
                style=""left: 48.08%; top: 38.08%; font-size: calc(var(--scale-factor)*12.00px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 53.32%; top: 38.11%; font-size: calc(var(--scale-factor)*12.00px); font-family: serif;""
                role=""presentation"" dir=""ltr"">終</span><span
                style=""left: 55.33%; top: 38.11%; font-size: calc(var(--scale-factor)*12.00px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 60.35%; top: 38.11%; font-size: calc(var(--scale-factor)*12.00px); font-family: serif;""
                role=""presentation"" dir=""ltr"">了</span><br role=""presentation""><span
                style=""left: 51.16%; top: 9.18%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr"">保</span><span
                style=""left: 53.01%; top: 9.18%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 54.44%; top: 9.18%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr"">険</span><span
                style=""left: 56.3%; top: 9.18%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 57.72%; top: 9.18%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr"">者</span><span
                style=""left: 59.58%; top: 9.18%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 61.03%; top: 9.18%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr"">番</span><span
                style=""left: 62.88%; top: 9.18%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 64.31%; top: 9.18%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr"">号</span><br role=""presentation""><span
                style=""left: 67.61%; top: 38.11%; font-size: calc(var(--scale-factor)*12.00px); font-family: serif;""
                role=""presentation"" dir=""ltr"">転</span><span
                style=""left: 69.62%; top: 38.11%; font-size: calc(var(--scale-factor)*12.00px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 74.64%; top: 38.11%; font-size: calc(var(--scale-factor)*12.00px); font-family: serif;""
                role=""presentation"" dir=""ltr"">帰</span><br role=""presentation""><span
                style=""left: 68.29%; top: 42.17%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr"">・</span><span
                style=""left: 70.15%; top: 42.17%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 72.15%; top: 42.17%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr"">・</span><span
                style=""left: 74%; top: 42.17%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 75.97%; top: 42.17%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr"">・</span><br role=""presentation""><span
                style=""left: 81.39%; top: 38.81%; font-size: calc(var(--scale-factor)*12.00px); font-family: serif;""
                role=""presentation"" dir=""ltr"">予</span><span
                style=""left: 83.41%; top: 38.81%; font-size: calc(var(--scale-factor)*12.00px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 85.87%; top: 38.81%; font-size: calc(var(--scale-factor)*12.00px); font-family: serif;""
                role=""presentation"" dir=""ltr"">定</span><span
                style=""left: 87.89%; top: 38.81%; font-size: calc(var(--scale-factor)*12.00px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 90.35%; top: 38.81%; font-size: calc(var(--scale-factor)*12.00px); font-family: serif;""
                role=""presentation"" dir=""ltr"">日</span><br role=""presentation""><span
                style=""left: 65.55%; top: 42.2%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif; transform: scaleX(1.02004);""
                role=""presentation"" dir=""ltr"">治ゆ</span><span
                style=""left: 68.64%; top: 42.2%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 69.6%; top: 42.2%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif; transform: scaleX(1.07069);""
                role=""presentation"" dir=""ltr"">死亡 中止</span><br role=""presentation""><span
                style=""left: 65.55%; top: 45.82%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif; transform: scaleX(1.02004);""
                role=""presentation"" dir=""ltr"">治ゆ</span><span
                style=""left: 68.64%; top: 45.82%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 69.6%; top: 45.82%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif; transform: scaleX(1.07069);""
                role=""presentation"" dir=""ltr"">死亡 中止</span><br role=""presentation""><span
                style=""left: 65.55%; top: 49.37%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif; transform: scaleX(1.02004);""
                role=""presentation"" dir=""ltr"">治ゆ</span><span
                style=""left: 68.64%; top: 49.37%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 69.6%; top: 49.37%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif; transform: scaleX(1.07069);""
                role=""presentation"" dir=""ltr"">死亡 中止</span><br role=""presentation""><span
                style=""left: 65.55%; top: 52.92%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif; transform: scaleX(1.02004);""
                role=""presentation"" dir=""ltr"">治ゆ</span><span
                style=""left: 68.64%; top: 52.92%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 69.6%; top: 52.92%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif; transform: scaleX(1.07069);""
                role=""presentation"" dir=""ltr"">死亡 中止</span><br role=""presentation""><span
                style=""left: 65.55%; top: 56.54%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif; transform: scaleX(1.02004);""
                role=""presentation"" dir=""ltr"">治ゆ</span><span
                style=""left: 68.64%; top: 56.54%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 69.6%; top: 56.54%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif; transform: scaleX(1.07069);""
                role=""presentation"" dir=""ltr"">死亡 中止</span><br role=""presentation""><span
                style=""left: 65.55%; top: 60.23%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif; transform: scaleX(1.02004);""
                role=""presentation"" dir=""ltr"">治ゆ</span><span
                style=""left: 68.64%; top: 60.23%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 69.6%; top: 60.23%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif; transform: scaleX(1.07069);""
                role=""presentation"" dir=""ltr"">死亡 中止</span><br role=""presentation""><span
                style=""left: 65.55%; top: 63.85%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif; transform: scaleX(1.02004);""
                role=""presentation"" dir=""ltr"">治ゆ</span><span
                style=""left: 68.64%; top: 63.85%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 69.6%; top: 63.85%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif; transform: scaleX(1.07069);""
                role=""presentation"" dir=""ltr"">死亡 中止</span><br role=""presentation""><span
                style=""left: 65.55%; top: 67.46%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif; transform: scaleX(1.02004);""
                role=""presentation"" dir=""ltr"">治ゆ</span><span
                style=""left: 68.64%; top: 67.46%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 69.6%; top: 67.46%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif; transform: scaleX(1.07069);""
                role=""presentation"" dir=""ltr"">死亡 中止</span><br role=""presentation""><span
                style=""left: 65.55%; top: 71.08%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif; transform: scaleX(1.02004);""
                role=""presentation"" dir=""ltr"">治ゆ</span><span
                style=""left: 68.64%; top: 71.08%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 69.6%; top: 71.08%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif; transform: scaleX(1.07069);""
                role=""presentation"" dir=""ltr"">死亡 中止</span><br role=""presentation""><span
                style=""left: 65.55%; top: 74.63%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif; transform: scaleX(1.02004);""
                role=""presentation"" dir=""ltr"">治ゆ</span><span
                style=""left: 68.64%; top: 74.63%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 69.6%; top: 74.63%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif; transform: scaleX(1.07069);""
                role=""presentation"" dir=""ltr"">死亡 中止</span><br role=""presentation""><span
                style=""left: 35.07%; top: 80.06%; font-size: calc(var(--scale-factor)*8.04px); font-family: serif;""
                role=""presentation"" dir=""ltr"">労</span><span
                style=""left: 36.42%; top: 80.06%; font-size: calc(var(--scale-factor)*8.04px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 38.4%; top: 80.06%; font-size: calc(var(--scale-factor)*8.04px); font-family: serif;""
                role=""presentation"" dir=""ltr"">務</span><span
                style=""left: 39.75%; top: 80.06%; font-size: calc(var(--scale-factor)*8.04px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 41.73%; top: 80.06%; font-size: calc(var(--scale-factor)*8.04px); font-family: serif;""
                role=""presentation"" dir=""ltr"">不</span><span
                style=""left: 43.08%; top: 80.06%; font-size: calc(var(--scale-factor)*8.04px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 45.05%; top: 80.06%; font-size: calc(var(--scale-factor)*8.04px); font-family: serif;""
                role=""presentation"" dir=""ltr"">能</span><span
                style=""left: 46.4%; top: 80.06%; font-size: calc(var(--scale-factor)*8.04px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 48.38%; top: 80.06%; font-size: calc(var(--scale-factor)*8.04px); font-family: serif;""
                role=""presentation"" dir=""ltr"">期</span><span
                style=""left: 49.73%; top: 80.06%; font-size: calc(var(--scale-factor)*8.04px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 51.7%; top: 80.06%; font-size: calc(var(--scale-factor)*8.04px); font-family: serif;""
                role=""presentation"" dir=""ltr"">間</span><br role=""presentation""><span
                style=""left: 8.16%; top: 92.51%; font-size: calc(var(--scale-factor)*8.04px); font-family: serif; transform: scaleX(1.14684);""
                role=""presentation"" dir=""ltr"">備 考</span><br role=""presentation""><span
                style=""left: 7.9%; top: 90.06%; font-size: calc(var(--scale-factor)*8.04px); font-family: serif; transform: scaleX(0.875866);""
                role=""presentation"" dir=""ltr"">疑いがある場合は、その旨</span><br role=""presentation""><span
                style=""left: 34.05%; top: 37.37%; font-size: calc(var(--scale-factor)*12.00px); font-family: serif;""
                role=""presentation"" dir=""ltr"">職</span><br role=""presentation""><span
                style=""left: 71.66%; top: 2.88%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif; transform: scaleX(1.00004);""
                role=""presentation"" dir=""ltr"">発行日時</span><br role=""presentation""><span
                style=""left: 77.5%; top: 42.23%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif;""
                role=""presentation"" dir=""ltr"">他</span><br role=""presentation""><span
                style=""left: 68.29%; top: 45.77%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr"">・</span><span
                style=""left: 70.15%; top: 45.77%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 72.15%; top: 45.77%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr"">・</span><span
                style=""left: 74%; top: 45.77%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 75.97%; top: 45.77%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr"">・</span><br role=""presentation""><span
                style=""left: 68.29%; top: 49.36%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr"">・</span><span
                style=""left: 70.15%; top: 49.36%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 72.15%; top: 49.36%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr"">・</span><span
                style=""left: 74%; top: 49.36%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 75.97%; top: 49.36%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr"">・</span><br role=""presentation""><span
                style=""left: 68.29%; top: 52.95%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr"">・</span><span
                style=""left: 70.15%; top: 52.95%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 72.15%; top: 52.95%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr"">・</span><span
                style=""left: 74%; top: 52.95%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 75.97%; top: 52.95%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr"">・</span><br role=""presentation""><span
                style=""left: 68.29%; top: 56.54%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr"">・</span><span
                style=""left: 70.15%; top: 56.54%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 72.15%; top: 56.54%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr"">・</span><span
                style=""left: 74%; top: 56.54%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 75.97%; top: 56.54%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr"">・</span><br role=""presentation""><span
                style=""left: 68.29%; top: 60.13%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr"">・</span><span
                style=""left: 70.15%; top: 60.13%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 72.15%; top: 60.13%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr"">・</span><span
                style=""left: 74%; top: 60.13%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 75.97%; top: 60.13%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr"">・</span><br role=""presentation""><span
                style=""left: 68.29%; top: 63.73%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr"">・</span><span
                style=""left: 70.15%; top: 63.73%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 72.15%; top: 63.73%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr"">・</span><span
                style=""left: 74%; top: 63.73%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 75.97%; top: 63.73%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr"">・</span><br role=""presentation""><span
                style=""left: 68.29%; top: 67.32%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr"">・</span><span
                style=""left: 70.15%; top: 67.32%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 72.15%; top: 67.32%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr"">・</span><span
                style=""left: 74%; top: 67.32%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 75.97%; top: 67.32%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr"">・</span><br role=""presentation""><span
                style=""left: 68.29%; top: 70.91%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr"">・</span><span
                style=""left: 70.15%; top: 70.91%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 72.15%; top: 70.91%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr"">・</span><span
                style=""left: 74%; top: 70.91%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 75.97%; top: 70.91%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr"">・</span><br role=""presentation""><span
                style=""left: 68.29%; top: 74.5%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr"">・</span><span
                style=""left: 70.15%; top: 74.5%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 72.15%; top: 74.5%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr"">・</span><span
                style=""left: 74%; top: 74.5%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 75.97%; top: 74.5%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr"">・</span><br role=""presentation""><span
                style=""left: 77.5%; top: 45.82%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif;""
                role=""presentation"" dir=""ltr"">他</span><br role=""presentation""><span
                style=""left: 77.5%; top: 49.37%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif;""
                role=""presentation"" dir=""ltr"">他</span><br role=""presentation""><span
                style=""left: 77.5%; top: 52.92%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif;""
                role=""presentation"" dir=""ltr"">他</span><br role=""presentation""><span
                style=""left: 77.5%; top: 56.54%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif;""
                role=""presentation"" dir=""ltr"">他</span><br role=""presentation""><span
                style=""left: 77.5%; top: 60.22%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif;""
                role=""presentation"" dir=""ltr"">他</span><br role=""presentation""><span
                style=""left: 77.5%; top: 63.84%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif;""
                role=""presentation"" dir=""ltr"">他</span><br role=""presentation""><span
                style=""left: 77.5%; top: 67.46%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif;""
                role=""presentation"" dir=""ltr"">他</span><br role=""presentation""><span
                style=""left: 77.5%; top: 71.08%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif;""
                role=""presentation"" dir=""ltr"">他</span><br role=""presentation""><span
                style=""left: 77.5%; top: 74.63%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif;""
                role=""presentation"" dir=""ltr"">他</span><br role=""presentation""><span
                style=""left: 19.98%; top: 24.09%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif; transform: scaleX(0.996707);""
                role=""presentation"" dir=""ltr"">兵庫県加古郡稲美町国岡１</span><span
                style=""left: 42.15%; top: 24.09%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr"">－</span><span
                style=""left: 44%; top: 24.09%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif; transform: scaleX(0.996707);""
                role=""presentation"" dir=""ltr"">１４８</span><br role=""presentation""><span
                style=""left: 37.15%; top: 19.53%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif; transform: scaleX(1.10605);""
                role=""presentation"" dir=""ltr"">75</span><span
                style=""left: 19.98%; top: 19.48%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr"">昭</span><span
                style=""left: 21.83%; top: 19.48%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr"">和</span><span
                style=""left: 23.68%; top: 19.48%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 24.19%; top: 19.48%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif; transform: scaleX(1.10605);""
                role=""presentation"" dir=""ltr"">22</span><span
                style=""left: 26.24%; top: 19.48%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif;""
                role=""presentation"" dir=""ltr"">年</span><span
                style=""left: 28.1%; top: 19.48%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif; transform: scaleX(1.05062);""
                role=""presentation"" dir=""ltr"">02月10日</span><br role=""presentation""><span
                style=""left: 61.9%; top: 11.91%; font-size: calc(var(--scale-factor)*9.96px); font-family: serif;""
                role=""presentation"" dir=""ltr"">・</span><br role=""presentation""><span
                style=""left: 19.98%; top: 15.02%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif; transform: scaleX(1.03528);""
                role=""presentation"" dir=""ltr""></span><br role=""presentation""><span
                style=""left: 20.5%; top: 6.49%; font-size: calc(var(--scale-factor)*14.04px); font-family: serif; transform: scaleX(1.11156);""
                role=""presentation"" dir=""ltr""></span><br role=""presentation""><span
                style=""left: 19.98%; top: 16.99%; font-size: calc(var(--scale-factor)*12.00px); font-family: serif; transform: scaleX(1.00004);""
                role=""presentation"" dir=""ltr""></span><span
                style=""left: 24.01%; top: 16.99%; font-size: calc(var(--scale-factor)*12.00px); font-family: serif;""
                role=""presentation"" dir=""ltr""> </span><span
                style=""left: 26.02%; top: 16.99%; font-size: calc(var(--scale-factor)*12.00px); font-family: serif; transform: scaleX(1.10604);""
                role=""presentation"" dir=""ltr"">91</span><br role=""presentation""><span
                style=""left: 47.43%; top: 19.51%; font-size: calc(var(--scale-factor)*12.00px); font-family: serif;""
                role=""presentation"" dir=""ltr"">女</span><br role=""presentation""><span
                style=""left: 19.98%; top: 28.42%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif; transform: scaleX(1.12837);""
                role=""presentation"" dir=""ltr"">
                ");
            sb.Append(karte1Data.PtTel);



            sb.Append(@"</span><br role=""presentation""><span
                style=""left: 22.46%; top: 21.76%; font-size: calc(var(--scale-factor)*11.04px); font-family: serif; transform: scaleX(1.14801);""
                role=""presentation"" dir=""ltr"">675-1115</span>
                <br role=""presentation"">

             
                <span style=""left: 8.06%; top: 42.17%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif; transform: scaleX(1.00004);""
                role=""presentation"" dir=""ltr"">");

            if (karte1Data.ListByomeis.Count > 0)
                sb.Append(karte1Data.ListByomeis[0]?.Byomei ?? string.Empty);
            sb.Append(@"</span><br role=""presentation""><span
                style=""left: 8.06%; top: 45.79%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif; transform: scaleX(1.00004);""
                role=""presentation"" dir=""ltr"">");
            if (karte1Data.ListByomeis.Count > 1)
                sb.Append(karte1Data.ListByomeis[1]?.Byomei ?? string.Empty);
            sb.Append(@"</span><br role=""presentation""><span
                style=""left: 8.06%; top: 49.41%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif; transform: scaleX(1.00004);""
                role=""presentation"" dir=""ltr"">");
            if (karte1Data.ListByomeis.Count > 2)
                sb.Append(karte1Data.ListByomeis[2]?.Byomei ?? string.Empty);

            sb.Append(@"
            </span><br role=""presentation""><span
                style=""left: 8.06%; top: 53.03%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif; transform: scaleX(1.00004);""
                role=""presentation"" dir=""ltr"">");
            if (karte1Data.ListByomeis.Count > 3)
                sb.Append(karte1Data.ListByomeis[3]?.Byomei ?? string.Empty);
            sb.Append(@"
            </span><br role=""presentation""><span
                style=""left: 8.06%; top: 56.64%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif; transform: scaleX(1.00004);""
                role=""presentation"" dir=""ltr"">");
            if (karte1Data.ListByomeis.Count > 4)
                sb.Append(karte1Data.ListByomeis[4]?.Byomei ?? string.Empty);
            sb.Append(@"</span><br role=""presentation""><span
                style=""left: 8.06%; top: 60.26%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif; transform: scaleX(1.00004);""
                role=""presentation"" dir=""ltr"">");

            if (karte1Data.ListByomeis.Count > 5)
                sb.Append(karte1Data.ListByomeis[5]?.Byomei ?? string.Empty);

            sb.Append(@"
            </span><br role=""presentation""><span
                style=""left: 8.06%; top: 63.88%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif; transform: scaleX(1.00004);""
                role=""presentation"" dir=""ltr"">");
            if (karte1Data.ListByomeis.Count > 6)
                sb.Append(karte1Data.ListByomeis[6]?.Byomei ?? string.Empty);
            sb.Append(@"   
                </span><br role=""presentation""><span
                style=""left: 8.06%; top: 67.5%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif; transform: scaleX(1.00004);""
                role=""presentation"" dir=""ltr"">");

            if (karte1Data.ListByomeis.Count > 7)
                sb.Append(karte1Data.ListByomeis[7]?.Byomei ?? string.Empty);
            sb.Append(@"
</span><br role=""presentation""><span
                style=""left: 8.06%; top: 71.11%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif; transform: scaleX(1.00004);""
                role=""presentation"" dir=""ltr"">");
            if (karte1Data.ListByomeis.Count > 8)
                sb.Append(karte1Data.ListByomeis[8]?.Byomei ?? string.Empty);
            sb.Append(@"
</span><br role=""presentation""><span
                style=""left: 8.06%; top: 74.73%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif; transform: scaleX(1.00004);""
                role=""presentation"" dir=""ltr"">");
            if (karte1Data.ListByomeis.Count > 8)
                sb.Append(karte1Data.ListByomeis[8]?.Byomei ?? string.Empty);


            sb.Append(@"
</span><br role=""presentation"">

                <span style=""left: 37.23%; top: 42.17%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif; transform: scaleX(1.03125);""
                role=""presentation"" dir=""ltr"">");
            if (karte1Data.ListByomeis.Count > 0)
                sb.Append(karte1Data.ListByomeis[0].ByomeiStartDateWFormat);
            sb.Append(@"</span><br role=""presentation"">
               <span style=""left: 37.23%; top: 45.79%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif; transform: scaleX(1.03125);""
                role=""presentation"" dir=""ltr"">");
            if (karte1Data.ListByomeis.Count > 1)
                sb.Append(karte1Data.ListByomeis[1].ByomeiStartDateWFormat);
            sb.Append(@"</span><br role=""presentation""><span
                style=""left: 37.23%; top: 49.41%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif; transform: scaleX(1.03125);""
                role=""presentation"" dir=""ltr"">");
            if (karte1Data.ListByomeis.Count > 2)
                sb.Append(karte1Data.ListByomeis[2].ByomeiStartDateWFormat);
            sb.Append(@"
</span><br role=""presentation""><span
                style=""left: 37.23%; top: 53.03%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif; transform: scaleX(1.03125);""
                role=""presentation"" dir=""ltr"">");
            if (karte1Data.ListByomeis.Count > 3)
                sb.Append(karte1Data.ListByomeis[3].ByomeiStartDateWFormat);
            sb.Append(@"
</span><br role=""presentation""><span
                style=""left: 37.23%; top: 56.64%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif; transform: scaleX(1.03125);""
                role=""presentation"" dir=""ltr"">");
            if (karte1Data.ListByomeis.Count > 4)
                sb.Append(karte1Data.ListByomeis[4].ByomeiStartDateWFormat);
            sb.Append(@"
            </span><br role=""presentation""><span
                style=""left: 37.23%; top: 60.26%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif; transform: scaleX(1.03125);""
                role=""presentation"" dir=""ltr"">");
            if (karte1Data.ListByomeis.Count > 5)
                sb.Append(karte1Data.ListByomeis[5].ByomeiStartDateWFormat);
            sb.Append(@"
</span><br role=""presentation""><span
                style=""left: 37.23%; top: 63.88%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif; transform: scaleX(1.03125);""
                role=""presentation"" dir=""ltr"">
");
            if (karte1Data.ListByomeis.Count > 6)
                sb.Append(karte1Data.ListByomeis[6].ByomeiStartDateWFormat);
            sb.Append(@"
</span><br role=""presentation""><span
                style=""left: 37.23%; top: 67.5%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif; transform: scaleX(1.03125);""
                role=""presentation"" dir=""ltr"">
");
            if (karte1Data.ListByomeis.Count > 7)
                sb.Append(karte1Data.ListByomeis[7].ByomeiStartDateWFormat);
            sb.Append(@"
</span><br role=""presentation""><span
                style=""left: 37.23%; top: 71.11%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif; transform: scaleX(1.03125);""
                role=""presentation"" dir=""ltr"">");
            if (karte1Data.ListByomeis.Count > 8)
                sb.Append(karte1Data.ListByomeis[8].ByomeiStartDateWFormat);
            sb.Append(@"
            </span><br role=""presentation""><span
                style=""left: 37.23%; top: 74.73%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif; transform: scaleX(1.03125);""
                role=""presentation"" dir=""ltr"">");
            if (karte1Data.ListByomeis.Count > 9)
                sb.Append(karte1Data.ListByomeis[9].ByomeiStartDateWFormat);
            sb.Append(@"
            </span><br role=""presentation""><span
                style=""left: 51.42%; top: 42.17%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif; transform: scaleX(1.03125);""
                role=""presentation"" dir=""ltr"">");
            if (karte1Data.ListByomeis.Count > 0)
                sb.Append(karte1Data.ListByomeis[0].ByomeiTenkiDateWFormat);
            sb.Append(@"
            </span><br role=""presentation""><span
                style=""left: 51.42%; top: 45.79%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif; transform: scaleX(1.03125);""
                role=""presentation"" dir=""ltr"">");

            if (karte1Data.ListByomeis.Count > 2)
                sb.Append(karte1Data.ListByomeis[2].ByomeiTenkiDateWFormat);

            sb.Append(@"</span><br role=""presentation""><span
                style=""left: 51.42%; top: 53.03%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif; transform: scaleX(1.03125);""
                role=""presentation"" dir=""ltr"">");
            if (karte1Data.ListByomeis.Count > 3)
                sb.Append(karte1Data.ListByomeis[3].ByomeiTenkiDateWFormat);
            sb.Append(@"
            </span><br role=""presentation""><span
                style=""left: 73.67%; top: 41.99%; font-size: calc(var(--scale-factor)*14.04px); font-family: serif;""
                role=""presentation"" dir=""ltr"">〇</span><br role=""presentation""><span
                style=""left: 73.67%; top: 45.61%; font-size: calc(var(--scale-factor)*14.04px); font-family: serif;""
                role=""presentation"" dir=""ltr"">〇</span><br role=""presentation""><span
                style=""left: 73.67%; top: 52.84%; font-size: calc(var(--scale-factor)*14.04px); font-family: serif;""
                role=""presentation"" dir=""ltr"">〇</span><br role=""presentation""><span
                style=""left: 78.51%; top: 2.95%; font-size: calc(var(--scale-factor)*9.00px); font-family: serif; transform: scaleX(1.15792);""
                role=""presentation"" dir=""ltr""> ");
            sb.Append(karte1Data.SysDateTimeS);
            sb.Append(@"</span>
            <div class=""endOfContent""></div>
        </div>
        <div class=""annotationEditorLayer"" tabindex=""0"" style=""pointer-events: none; width: 992px; height: 1403px;""
            data-main-rotation=""0""></div>
    </div>
</body>

</html>");

            return sb.ToString();
        }
    }
}
